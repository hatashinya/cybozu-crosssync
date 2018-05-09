Cybozu CrossSync
================

2つのサイボウズ製品のスケジュールを同期

概要
----
Cybozu CrossSync は2つのサイボウズ製品のスケジュールをバックグラウンドで同期します。スケジュールの同期にはサイボウズの連携APIを使用します。サーバーマシンにではなく、 **クライアントのPCにインストール** します。

**[スクリーンショット](https://github.com/hatashinya/cybozu-crosssync/wiki/ScreenShot)**

ライセンス
---------
[GNU GPL v2](http://www.gnu.org/licenses/old-licenses/gpl-2.0.html)

機能
----
* 指定した2つのサイボウズ製品のスケジュールをバックグラウンドで同期
* 同期させる予定の種類、および同期を行う時間間隔を設定可
* 「今すぐ同期」ボタンの押下による即時同期

必要システム
------------
### サーバー側
* 以下のいずれかのサイボウズ製品
  * [サイボウズ(R) Office(R)](https://products.cybozu.co.jp/office/) Version 8.1.0 以降
  * [サイボウズ(R) ガルーン(R)](https://garoon.cybozu.co.jp/) Version 3.0.0 以降
  * サイボウズ(R) Office(R) on [cybozu.com](https://www.cybozu.com/)
  * Garoon(R) on [cybozu.com](https://www.cybozu.com/)

### クライアント側
* .NET Framework 4.5

動作確認済みOS
--------------
### クライアント側
* Windows 10

インストール
------------
サーバーマシンではなく、 **クライアントのPCにインストール** します。

1. **[Releases](https://github.com/hatashinya/cybozu-crosssync/releases)** ページからインストーラー（ **CrossSync-1.0.*.msi** ）をダウンロードします。
2. ダウンロードしたインストーラーをダブルクリックして起動します。
3. インストーラーにしたがって、以下の項目を指定します。
   * インストール場所
   * スタートアップフォルダにショートカット入れるかどうか
   * インストール完了時にアプリケーションを起動するかどうか

URLの指定について
-----------------
同期するサイボウズ製品のURLの指定方法は以下の通りとなります。

* サイボウズ(R) Office(R)
  * 例） `http://example.com/scripts/cbag/ag.exe`
  * 例） `http://example.com/cgi-bin/cbag/ag.cgi`
* サイボウズ(R) ガルーン(R)
  * 例） `http://example.com/scripts/cbgrn/grn.exe`
  * 例） `http://example.com/cgi-bin/cbgrn/grn.cgi`
* サイボウズ(R) Office(R) on cybozu.com
  * 例） `https://example.cybozu.com/o/ag.cgi`
* Garoon on cybozu.com
  * 例） `https://example.cybozu.com/g/`

リファレンス
------------
* [使用方法](https://github.com/hatashinya/cybozu-crosssync/wiki/Usage)
* [設定可能な項目](https://github.com/hatashinya/cybozu-crosssync/wiki/SettingItems)
* [同期の仕様](https://github.com/hatashinya/cybozu-crosssync/wiki/SyncSpec)

セキュリティ
------------
* APIアクセスの度に、ログイン名およびパスワードをプレーンテキストでサーバーに送信しています。セキュリティを確保するためには、SSLをご利用ください。 
* [cybozu.com](https://www.cybozu.com/) はSSLにて提供されております。

開発環境
--------
* Visual Studio 2017

開発・提供元
------------
[サイボウズ・ラボ株式会社](http://labs.cybozu.co.jp/)

著作権
------
Copyright(C) 2011 [Cybozu Labs, Inc.](http://labs.cybozu.co.jp/)

備考
----
* Cybozu CrossSync はサイボウズの **サポートの対象外** となります。あらかじめご了承ください。
