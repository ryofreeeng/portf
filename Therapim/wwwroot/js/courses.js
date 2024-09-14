//コースの補足情報表示用
const courseAddInfoNavis = document.querySelectorAll('.course-addInfo-navi');
const courseAddInfo = document.querySelector('.course-addInfo');
const courseAddInfoIcon = document.querySelector('.course-addInfo-icon');

//すべてのナビに対して表示非表示の切り替えイベントを付与
courseAddInfoNavis.forEach(courseAddInfoNavi => {
	courseAddInfoNavi.addEventListener('click', function(event) {
		//直前の要素を取得
	    var courseAddInfo = courseAddInfoNavi.previousElementSibling;
		//正しい要素を取得している場合のみ処理
		if (courseAddInfo.classList.contains('course-addInfo')) {
		    // 取得した要素が表示状態の場合は非表示にする
		    if (courseAddInfo.classList.contains('open')) {
		        courseAddInfo.classList.remove('open');
				setTimeout(() => {
				    courseAddInfoNavi.innerHTML = '詳細を見る<span class="course-addInfo-icon"></span>';
				}, 500);
		    // 取得した要素が非表示状態の場合は表示する
		    } else {
				courseAddInfo.classList.add('open');
				setTimeout(() => {
				    courseAddInfoNavi.innerHTML = '詳細を閉じる<span class="course-addInfo-icon open"></span>';
				}, 350);
		    }
		}
	});
});










