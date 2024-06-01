@REM
@REM ビルド用のランチャーです。
@REM
@REM 【使用方法】
@REM  make を実行すると、アプリケーション本体がビルドされます。
@REM
@REM 【注意】
@REM  * Rust によって記述されています。rustup が必要です。
@REM

@SET PATH=C:¥Program Files (x86)¥Microsoft Visual Studio¥2019¥Community¥MSBuild¥Current¥Bin;%PATH%

@cargo fmt
@cargo run -- %*
