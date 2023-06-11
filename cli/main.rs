//!
//! アプリケーションのエントリーポイントです。
//!

mod application;
mod util;

/// アプリケーションのエントリーポイントです。
fn main() {
	let args: Vec<String> = std::env::args().skip(1).collect();

	let mut options = getopts::Options::new();
	options.optflag("h", "help", "Show usage.");
	options.optflag("", "build", "Build application. (DEFAULT)");
	options.optflag("", "print", "Launch printer.");
	options.optflag("", "dryrun", "Launch printer. (dry run)");

	let result = options.parse(args);
	if result.is_err() {
		eprint!("{}", options.usage(""));
		std::process::exit(1);
	}
	let input = result.unwrap();

	if input.opt_present("help") {
		// ========== OPTIONAL: SHOW HELP ==========
		eprintln!("{}", options.usage("PtouchPrintSender brother ラベルプリンターにラベル出力を行います。"));
	} else if input.opt_present("build") {
		// ========== OPTIONAL: LAUNCH BUILD ==========
		// ビルド
		let result = application::run_build();
		if result.is_err() {
			// MSBuild のエラー
			exit_with_error(result.err().unwrap());
			return;
		}
	} else if input.opt_present("print") {
		// ========== OPTIONAL: LAUNCH PRINT ==========
		// アドレス帳ファイル(固定)
		let address_file = "ADDRESS.tsv";
		// option: dryrun
		let dryrun = input.opt_present("dryrun");
		// 印刷
		let result = application::run_print(address_file, dryrun);
		if result.is_err() {
			exit_with_error(result.err().unwrap());
			return;
		}
	} else {
		// ========== DEFAULT: LAUNCH BUILD ==========
		// ビルド
		let result = application::run_build();
		if result.is_err() {
			// MSBuild のエラー
			exit_with_error(result.err().unwrap());
			return;
		}
	}
}

/// エラー検出
fn exit_with_error(error: Box<dyn std::error::Error>) {
	error!("{}", error);
	std::process::exit(1);
}
