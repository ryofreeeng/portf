/* ホーム画面のよくある質問開閉処理 */

document.addEventListener('DOMContentLoaded', function() {

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
		});
	});    
});

