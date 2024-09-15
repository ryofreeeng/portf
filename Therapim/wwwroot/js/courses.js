document.addEventListener('DOMContentLoaded', function() {

	/* コースの補足情報表示用 */
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

	/* コースの画像表示用 */
    const thumbnails = document.querySelectorAll('.course-image-a'); //すべてのサムネイル    
    const modalBack = document.getElementById('modalBack'); //モーダル背景
    const modalImage = document.getElementById('modalImage'); //モーダル内の画像
    const prevButton = document.querySelector('.prev'); //←矢印
    const nextButton = document.querySelector('.next'); //→矢印
    
    let currentIndex = 0; //現在の表示画像の序列
    let thumnailsInCourse; //選択されたものと同じ親要素の中にあるサムネイル要素
    let thumnailCounts = 0; //選択されたコースのサムネイル数
    let thumnailIndex = 0; //選択されたサムネイルがコース内で何番目か

	//サムネイルがクリックされたらモーダルを表示
    thumbnails.forEach((thumbnail) => {
        thumbnail.addEventListener('click', function(event) {
        	//最も近い親要素を取得
            const parent = event.target.closest('#thumbnails');
			if (parent) {
				// 同じ親要素の中にあるサムネイル要素をすべて取得
				thumnailsInCourse = parent.querySelectorAll('.course-image-a');
				// 同じ親要素内のサムネイル要素の数
				thumnailCounts = thumnailsInCourse.length;
				// 選択されたサムネイル要素の親要素内の序列を取得
				thumnailIndex = Array.from(thumnailsInCourse).indexOf(event.target);
				//モーダルの表示と画像src置き換え処理                             
	            currentIndex = thumnailIndex;
	            updateModalImage();
	            modalBack.classList.add('open');
	        }
        });
    });
	//画像以外がクリックされたらモーダルを閉じる
    modalBack.addEventListener('click', (e) => {
        if(e.target != modalImage && e.target != prevButton && e.target != nextButton){
            modalBack.classList.remove('open');
        }
    });
	//矢印がクリックされたら表示画像のsrcを変更
    prevButton.addEventListener('click', () => {
        currentIndex = (currentIndex - 1 + thumbnailCounts) % thumnailCounts;
        updateModalImage();
    });
    nextButton.addEventListener('click', () => {
        currentIndex = (currentIndex + 1) % thumnailCounts;
        updateModalImage();
    });
    function updateModalImage() {
        modalImage.src = thumnailsInCourse[currentIndex].src;
    }
    
    
    
    
});

