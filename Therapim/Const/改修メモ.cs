//ただの改修メモ


/*
 
必須要件
・ログイン画面への遷移時に、クッキーにユーザIDが存在する場合はTOPへまたは遷移元へリダイレクトに変更
・BAseControllerクラスでセッション情報やクッキー情報はセットする

リファクタ
・ログ出力は別メソッド化したい
・コントローラ名、アクション名取得は別メソッド化したい


やること
・git登録
・Azureデプロイと更新反映
・例外処理を追加していく



・動作確認と項目抜け漏れ確認（スプレッドシートも）
　ログインとログアウト
　レビュー一覧表示
　レビュー投稿と編集　ログイン状態と非ログイン状態での挙動
・各処理パターンでの画面遷移とエラー画面表示を正しく行う
・ログの出力（特に実行過程と、APIからのエラーメッセージ等）





実施済メモ
・json変換について
            //リクエスト文字列を作成　System.Text.Jsonの機能だと自動で先頭が小文字に変換されてしまうためnewtonsoft.jsonに変えた
            //var jsonContent = JsonContent.Create(model);
            //var jsonContentString = await jsonContent.ReadAsStringAsync();

・悪意のあるユーザ対策
　ユーザIDやセッションIDは画面側では見れないようにする。
　　バックの処理で追加してAPIへ送る
　　クッキーは暗号化して保存する
　他人のレビューを編集できないように対策をする
　　レビュー編集時にユーザIDの一致をチェック
　　自分のユーザIDはクッキーに暗号化して保存することで分からないようにする

・APIから取得するレビュー情報にはエラーメッセージ等も含まれているが、編集画面と紐づけるモデルクラスにはそれらはない
また、レビュー自体の情報についても画面には出さないものもあるため、レビューのリクエストクラスとレスポンスクラスの項目や構造に差異がある
　編集画面表示前のレビュー取得メソッドは、２段階に分けて、レスポンス型を取得と、それを画面用リクエストクラスに変換する処理、にした
　同じメソッド内でもできることだが、一応分けた

・ログインフィルター処理
　単なる[LoginAuthorize]だけだと依存性の注入がうまくされずログ出力機能などを実行できなかった（program.csでサービス登録を適切にすればできたのかも？）
　そこで[TypeFilter(typeof(LoginAuthorizeAttribute))]属性を使用するようにした
　このときprogram.csでbuilder.Services.AddScoped<LoginAuthorizeAttribute>();によりDIコンテナにフィルターを登録　⇒これはServiceFilterの場合だけ必要だったので不要になった
　また、以下をすればすべてのアクションメソッドに対して適用できるが、ここでは不要のためしていない　⇒これもたぶんServiceFilterの場合の話？
    services.AddControllersWithViews(options =>
    {
        options.Filters.AddService<LoginAuthorizeAttribute>();
    });
　LoginAuthorizeをタイプフィルターとして使用するには、IAuthorizationFilterの継承が必要だった
　単なるフィルターとして使うならAuthorizeAttributeを継承して[LoginAuthorize]でよさそうだが、ログ出力などがうまくいかなかったので、
　基本的にIAuthorizationFilterを継承して[TypeFilter(typeof(LoginAuthorizeAttribute))]で記述した方がよさそう

　TypeFilter
　　リクエストごとにこのフィルターのインスタンスが作成されるので基本的にTransientのライフタイム。logger等を使用するには、DIコンテナに登録しておく必要があるが、.NET8では標準でそれが登録されているようなので
　　特にprogram.csに追記はいらなかった。コンストラクタにパラメータを渡すことができる

　ServiceFilter
　　このフィルタークラスをDIコンテナに登録しておく必要があるので、program.csに、builder.Services.AddScoped<LoginAuthorizeAttribute>();　等の記述が必要
　　　これによりライフタイムの設定が可能。コンストラクタにパラメータを渡すことはできない？
　　Transient,Scoped,Singletonの順にライフタイムが長くなる

　AuthorizeAttribute
　　単純な認可処理用。コンストラクタにパラメータを渡すなどの複雑な処理を想定していないのであまり推奨されない（.NETのバージョンによってはできないのかも？）
　　ログ出力などをしたいのなら使わない方がいいかも
　　ランク判定などのやつも本当はフィルター機能を使った方がいいらしい


・ユーザの利便性
　セッション情報が消えてもCokkie情報が残っていればセッション情報を復活させるようにした
　ログインが必須のアクションへアクセスする際のフィルター内の処理として実装
　これによりサーバの再起動があってもログインしなおす必要がない。ただし名前などの個人情報はセッションに復活させていない状態にしている






 */