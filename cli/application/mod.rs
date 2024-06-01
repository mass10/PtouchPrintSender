//!
//! アプリケーションのコアロジック
//!

use crate::{info, util};

/// コマンドを実行します。
fn execute_command(commands: &[&str]) -> Result<(), Box<dyn std::error::Error>> {
	// コマンドへのパスと引数に分割
	let (command, arguments) = commands.split_first().unwrap();

	info!("コマンドを呼び出しています... [{}]", command);
	println!("-------------------------------------------------------");

	// コマンドを呼び出し
	let exit_status = std::process::Command::new(command).args(arguments).spawn()?.wait()?;
	if !exit_status.success() {
		let code = exit_status.code().unwrap();
		std::process::exit(code);
	}

	println!("-------------------------------------------------------");
	return Ok(());
}

/// MSBuild.exe のパスを返します。
fn detect_msbuild() -> String {
	return "MSBuild.exe".to_string();
}

/// ソリューションをビルドします。
///
/// # Arguments
/// * `name` - ビルドするソリューションファイル(.sln)へのパス。.csproj などを指定しても構わない。
fn build_solution(path: &str) -> Result<(), Box<dyn std::error::Error>> {
	let msbuild = detect_msbuild();
	execute_command(&[&msbuild, path, "/p:configuration=Release"])?;
	return Ok(());
}

/// アプリケーションを実行します。
pub fn run_build() -> Result<(), Box<dyn std::error::Error>> {
	info!("##### START BUILD #####");

	// ビルドの出力ファイル
	const OUT_PATH: &str = r"PtouchPrintSenderApp\bin\Release\PTouchPrintSender.exe";

	// 最初のタイムスタンプ
	let former_filetime = util::get_filetime(OUT_PATH).unwrap_or_default();

	// ソリューションをビルドします。
	build_solution(r"PtouchPrintSenderApp\PTouchPrintSender.sln")?;

	// ビルド後のタイムスタンプ
	let current_filetime = util::get_filetime(OUT_PATH)?;

	// 確認
	if former_filetime == current_filetime {
		info!("ファイル [{}] は更新されませんでした。[{}] >> [{}]", OUT_PATH, &former_filetime, &current_filetime);
		return Ok(());
	}
	info!("ファイル [{}] をビルドしました。[{}] >> [{}]", OUT_PATH, &former_filetime, &current_filetime);

	return Ok(());
}

/// 印刷アプリケーションを、もし必要ならビルドします。
fn build_if_needed() -> Result<(), Box<dyn std::error::Error>> {
	if util::exists_file(r"PtouchPrintSenderApp\bin\Release\PTouchPrintSender.exe") {
		// バイナリが存在する場合はビルドをスキップします。
		info!("バイナリファイルが存在します。ビルドをスキップします。(もし更新が必要なら、make を実行してください)");
		return Ok(());
	}

	// ソリューションをビルドします。
	build_solution(r"PtouchPrintSenderApp\PTouchPrintSender.sln")?;

	return Ok(());
}

/// 印刷を実行します。
pub fn run_print(path: &str, dryrun: bool) -> Result<(), Box<dyn std::error::Error>> {
	info!("##### START PRINT #####");

	// バイナリファイルが無い場合はビルドします。
	build_if_needed()?;

	let mut command: Vec<&str> = vec![];
	command.push(r"PtouchPrintSenderApp\bin\Release\PtouchPrintSender.exe");

	if dryrun {
		command.push("--dryrun");
	}

	if path != "" {
		command.push("--address-file");
		command.push(path);
	}

	execute_command(&command)?;

	return Ok(());
}
