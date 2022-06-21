//!
//! アプリケーションのエントリーポイントです。
//!

mod application;
mod util;

/// アプリケーションのエントリーポイントです。
fn main() {
	let result = application::run();
	if result.is_err() {
		error!("{}", result.unwrap_err());
		return;
	}
}
