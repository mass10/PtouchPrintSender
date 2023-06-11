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

/// ソリューションをビルドします。
///
/// # Arguments
/// * `name` - ビルドするソリューションファイル(.sln)へのパス。.csproj などを指定しても構わない。
fn build_solution(path: &str) -> Result<(), Box<dyn std::error::Error>> {
	let msbuild = util::getenv("PP_MSBUILD");
	execute_command(&[&msbuild, path, "/p:configuration=Release"])?;
	return Ok(());
}

/// アプリケーションを実行します。
pub fn run_build() -> Result<(), Box<dyn std::error::Error>> {
	info!("##### START BUILD #####");

	// ビルドの出力ファイル
	const OUT_PATH: &str = r#"bin\Release\PTouchPrintSender.exe"#;

	// 最初のタイムスタンプ
	let former_filetime = util::get_filetime(OUT_PATH).unwrap_or_default();

	// ソリューションをビルドします。
	build_solution("PTouchPrintSender.sln")?;

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

pub fn run_print(dryrun: bool) -> Result<(), Box<dyn std::error::Error>> {
	info!("##### START PRINT #####");

	let mut command: Vec<&str> = vec![];
	command.push(r"bin\Release\PtouchPrintSender.exe");
	if dryrun {
		command.push("--dryrun");
	}

	execute_command(&command)?;

	return Ok(());
}
