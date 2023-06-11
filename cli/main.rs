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
	options.optflag("", "build", "Build application.");
	options.optflag("", "print", "Launch printer.");
	options.optflag("", "dryrun", "print dry run.");

	let result = options.parse(args);
	if result.is_err() {
		eprint!("{}", options.usage(""));
		std::process::exit(1);
	}
	let input = result.unwrap();

	if input.opt_present("help") {
		// ========== OPTIONAL: SHOW HELP ==========
		eprintln!("{}", options.usage(""));
	} else if input.opt_present("build") {
		// ========== OPTIONAL: LAUNCH BUILD ==========
		let result = application::run_build();
		report_error(&result);
	} else if input.opt_present("print") {
		// ========== OPTIONAL: LAUNCH PRINT ==========
		let dryrun = input.opt_present("dryrun");
		let result = application::run_print(dryrun);
		report_error(&result);
	} else {
		// ========== DEFAULT: LAUNCH BUILD ==========
		let result = application::run_build();
		report_error(&result);
	}
}

/// エラー検出
fn report_error(result: &Result<(), Box<dyn std::error::Error>>) {
	if result.is_ok() {
		return;
	}
	let error = result.as_ref().unwrap_err();
	error!("{}", error);
	std::process::exit(1);
}
