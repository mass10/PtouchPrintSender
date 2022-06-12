/// コマンドを実行します。
#[allow(unused)]
fn execute_command(command: &[&str]) -> Result<(), Box<dyn std::error::Error>> {
	let (command_name, arguments) = command.split_first().unwrap();
	let exit_status = std::process::Command::new(command_name).args(arguments).spawn()?.wait()?;
	if !exit_status.success() {
		let code = exit_status.code().unwrap();
		std::process::exit(code);
	}
	return Ok(());
}

/// OS のシェル内でコマンドを実行します。
#[allow(unused)]
fn execute_shell_command(command: &[&str]) -> Result<(), Box<dyn std::error::Error>> {
	let exit_status = std::process::Command::new("cmd.exe").arg("/C").args(command).spawn()?.wait()?;
	if !exit_status.success() {
		let code = exit_status.code().unwrap();
		std::process::exit(code);
	}
	return Ok(());
}

/// std::time::SystemTime の文字列表現を返します。
fn format_filetime(time: &std::time::SystemTime) -> String {
	let timestamp = chrono::DateTime::<chrono::Local>::from(*time);
	return format!("{}", timestamp.format("%Y-%m-%d %H:%M:%S%.3f"));
}

/// ファイルのタイムスタンを返します。
fn get_filetime(path: &str) -> Result<String, Box<dyn std::error::Error>> {
	let path = std::path::Path::new(path);
	let metadata = std::fs::metadata(path)?;
	let system_time = metadata.modified()?;
	return Ok(format_filetime(&system_time));
}

/// 環境変数を取得する
fn getenv(name: &str) -> String {
	return std::env::var(name).unwrap_or_else(|_| "".to_string());
}

/// ソリューションをビルドします。
fn run() -> Result<(), Box<dyn std::error::Error>> {
	const MODULE_PATH: &str = r#"bin\Release\PTouchPrintSender.exe"#;

	// 最初のタイムスタンプ
	let former_filetime = get_filetime(MODULE_PATH).unwrap_or_default();

	// ソリューションをビルドします。
	let msbuild = getenv("PP_MSBUILD");
	execute_command(&[&msbuild, "PTouchPrintSender.sln", r#"/p:configuration=Release"#])?;

	// ビルド後のタイムスタンプ
	let current_filetime = get_filetime(MODULE_PATH)?;

	// 確認
	if former_filetime == current_filetime {
		println!("[INFO] ファイル [{}] はビルドされませんでした。[{}]", MODULE_PATH, current_filetime);
		return Ok(());
	}
	println!("[INFO] ファイル [{}] をビルドしました。[{}]", MODULE_PATH, &current_filetime);

	return Ok(());
}

/// アプリケーションのエントリーポイントです。
fn main() {
	let result = run();
	if result.is_err() {
		println!("Error: {}", result.unwrap_err());
		return;
	}
}
