//!
//! 各種ユーティリティ
//!

/// 簡易ロギング
#[macro_export]
macro_rules! info {
	($($arg:tt)*) => {
		let line = format!($($arg)*);
		println!("{} [INFO] {}", util::get_current_timestamp(), line);
	};
}

/// 簡易ロギング
#[macro_export]
macro_rules! error {
	($($arg:tt)*) => {
		let line = format!($($arg)*);
		println!("{} [ERROR] {}", util::get_current_timestamp(), line);
	};
}

/// std::time::SystemTime の文字列表現を返します。
///
/// # Arguments
/// * `time` - 文字列表現を取得する SystemTime オブジェクト。
///
/// # Returns
/// タイムスタンプ
pub fn format_filetime(time: &std::time::SystemTime) -> String {
	let timestamp = chrono::DateTime::<chrono::Local>::from(*time);
	return format!("{}", timestamp.format("%Y-%m-%d %H:%M:%S%.3f"));
}

/// システムのタイムスタンプを返します。
///
/// # Returns
/// システムのタイムスタンプ
pub fn get_current_timestamp() -> String {
	let time = chrono::Local::now();
	return format!("{}", time.format("%Y-%m-%d %H:%M:%S%.3f"));
}

/// ファイルのタイムスタンプを返します。
///
/// # Arguments
/// * `path` - ファイルへのパス。
///
/// # Returns
/// ファイルのタイムスタンプ
pub fn get_filetime(path: &str) -> Result<String, Box<dyn std::error::Error>> {
	let path = std::path::Path::new(path);
	let metadata = std::fs::metadata(path)?;
	let system_time = metadata.modified()?;
	return Ok(format_filetime(&system_time));
}

/// 環境変数を取得します。
///
/// # Returns
/// 環境変数
pub fn getenv(name: &str) -> String {
	return std::env::var(name).unwrap_or_else(|_| "".to_string());
}
