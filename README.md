# PtouchPrintSender 2021-10-23
　ブラザー工業 ラベルプリンター向け b-PAC(Brother P-touch Applicable Component、COM コンポーネント)を用いて、プリンター出力を行うコマンドラインアプリケーションのサンプルです。
 
# 補足
* あらかじめ、COM 参照をプロジェクトに追加しています。これにより、型やシンボルがある程度明らかになります。COM 参照を追加せずに、`dynamic` で実装することも可能です。実行時にダイナミックにロードするスタイルです。

# 注意点
* b-PAC 32bit 版コンポーネントを、あらかじめインストールしておく必要があります。

# 宛名.lbx
　印刷用のテンプレートファイルです。P-touch Editor で作成したものです。このファイルを実行時に読み込み、印刷を行います。
# リンク
* https://github.com/mass10/PtouchPrintSender
