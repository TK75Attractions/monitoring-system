## やりたいこと
- 監視対象アプリケーションが異常終了したときにNATSにメッセージをpublish(必須)
- 異常終了したときにエラーメッセージやminidump, coredump等をNATSで送る(できれば)

## 手段の検討
### 既存のサービス
- #### CrashPad
ドキュメントが充実してないし導入コストが高そうなので没
- #### Sentry
CrashPadのラッパーを提供しているSentryなどのSaaSも有料なので没

というわけで基本的に自前で実装する

### 自前実装
1つ目の機能に関してはcrash-detectorが監視対象アプリを子プロセスとして起動->異常終了したら終了ステータスが返ってくるので0じゃなかったらアラート、みたいな感じで割と簡単に実装できる

2つ目に関してはちょっと複雑で、dumpを取得するためにLinuxの場合はjournalctlとかcoredumpctl、Windowsの場合はWER(Windows Error Reporting)とかJob ObjectとかOS固有のAPIを触る必要がある  
GolangはOSのAPIを呼ぶのは不得手なので、やるならRustかC#かなぁ、という感じ  
Rustは多分特に苦もなく実装できるけど履修コストが高いのがネック
C#はもともとWindows専用と名高かったこともありWindowsのAPIを呼ぶのは得意なんだけど、ターゲットOSごとに分岐するのかRustほど自然にはできない

2つ目の機能は投擲ゲームとかDiscordにアラート流す部分とか必要なものを実装して余力があったらやる