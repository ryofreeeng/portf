/* ホーム画面のよくある質問開閉処理 */

document.addEventListener('DOMContentLoaded', function() {

	/* お知らせ読み込み中の表示 */
	const newsList = document.querySelector('.news-list');
	newsList.innerHTML = "お知らせ取得中...";
	
	/* お知らせを取得 */
	const params = new URLSearchParams();
	params.append("input", "");
	params.append("sqlCond", "");
	params.append("targetRange", "all");
	params.append("crudType", "read");
	params.append("targetFunc", "information");

	const requestParams = {
	  method: "POST",
	  headers: {
	    "Accept": "application/json",
	    "Content-Type": "application/x-www-form-urlencoded",
	  },
	  body: params,
	};

	fetch(GAS_API_URL, requestParams)
	  .then((response) => response.json())
	  .then((result) => {
	  
	  	// 順序並び替え
	    let SqlContent = result.SqlContent.sort((a, b) => {
	        return new Date(b.PublishedDateTime) - new Date(a.PublishedDateTime);
	    })	  	
	  	
	  	
	  	// 反映
		newsList.innerHTML = "";  
	  	for(let i = 0; i < SqlContent.length; i++){
			// 新しいリストアイテムを作成
			const listItem = document.createElement('li');
			// span要素を作成（クラス名 "english"）
			const spanEnglish = document.createElement('span');
			spanEnglish.className = 'english';
			spanEnglish.innerHTML = SqlContent[i].PublishedDateTime.split('T')[0]; // PublishedDateTimeを反映
			// strong要素を作成
			const strong = document.createElement('strong');
			strong.innerHTML = SqlContent[i].Title // Titleを反映
			// もう一つのspan要素を作成
			const spanOther = document.createElement('span');
			spanOther.innerHTML = SqlContent[i].Description; // Descriptionを反映
			// 構造を組み立てる
			listItem.appendChild(spanEnglish);
			listItem.appendChild(document.createElement('br')); // br要素を追加
			listItem.appendChild(strong);
			listItem.appendChild(document.createElement('br')); // 2つ目のbr要素を追加
			listItem.appendChild(spanOther);
			// newsListにリストアイテムを追加
			if (newsList) {
			    newsList.appendChild(listItem);
			}
		}
	})
	.catch((e) => console.log(e));


	/* コースの補足情報表示用 */
	const questions = document.querySelectorAll('.question');
	const courseAddInfo = document.querySelector('.course-addInfo');
	const courseAddInfoIcon = document.querySelector('.course-addInfo-icon');
	//すべてのナビに対して表示非表示の切り替えイベントを付与
	questions.forEach(question => {
		question.addEventListener('click', function(event) {
			//直後の要素を取得
		    var answer = question.nextElementSibling;
			//正しい要素を取得している場合のみ処理
			if (answer.classList.contains('answer')) {
			    // 取得した要素が表示状態の場合は非表示にする
			    if (answer.classList.contains('open')) {
			        answer.classList.remove('open');
			    // 取得した要素が非表示状態の場合は表示する
			    } else {
					answer.classList.add('open');
			    }
			}
		})
	})
})


// 未使用
function EncodeHTMLForm( data )
{
    var params = [];
    for( var name in data )
    {
        var value = data[ name ];
        var param = encodeURIComponent( name ) + '=' + encodeURIComponent( value );
        params.push( param );
    }
    return params.join( '&' ).replace( /%20/g, '+' );
}

