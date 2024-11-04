/* ホーム画面のよくある質問開閉処理 */

document.addEventListener('DOMContentLoaded', function() {
	/* お知らせ読み込み中の表示 */
	const newsList = document.querySelector('.news-list');
	newsList.innerHTML = "お知らせ取得中...";
	
	/* お知らせを取得 */
	const GAS_API_URL = "https://script.google.com/macros/s/AKfycbxc1XkXEhSXawl6IjqmqcmDq_Cgj7PY7Usl6HKsOhdrnq8ANFz1P8gVv3tJ5C49ZM81/exec";

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
});

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

