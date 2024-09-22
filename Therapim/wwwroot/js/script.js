
document.addEventListener('DOMContentLoaded', function() {
	// ヘッダー要素を取得
	var header = document.querySelector('header');
	// ヘッダーの高さを取得
	var headerHeight = header.offsetHeight;
	// ボディのpadding-topを設定
	document.body.style.paddingTop = headerHeight + 'px';
});

// スクロールイベントでヘッダーを非表示
let lastScrollTop = 0;
window.addEventListener('scroll', () => {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    if (scrollTop > lastScrollTop) {
        document.querySelector('header').style.top = '-100px'; // ヘッダーを上に隠す
    } else {
        document.querySelector('header').style.top = '0'; // ヘッダーを表示する
    }
    lastScrollTop = scrollTop;
});

// ハンバーガーメニューの開閉
const hamburger = document.querySelector('.hamburger-menu');
const navRow = document.querySelector('.nav-row');

hamburger.addEventListener('click', () => {
    navRow.classList.toggle('active');
    hamburger.querySelector('.hamburger').classList.toggle('open');
});

// ユーザーアイコンのポップアップメニュー
const userIcon = document.querySelector('.user-icon');
if(userIcon != undefined){
	userIcon.addEventListener('click', () => {
	    userIcon.classList.toggle('active');
	});
}

// ページの他の部分をクリックしたらメニューを閉じる
document.addEventListener('click', (event) => {
    if (!event.target.closest('.user-icon') && !event.target.closest('.user-menu-dropdown')) {
        userIcon.classList.remove('active');
    }

    if (!event.target.closest('.hamburger-menu') && !event.target.closest('.nav-row')) {
        navRow.classList.remove('active');
        hamburger.querySelector('.hamburger').classList.remove('open');
    }
});
