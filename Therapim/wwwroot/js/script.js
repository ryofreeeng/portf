
  document.addEventListener('DOMContentLoaded', function() {
    // ヘッダー要素を取得
    var header = document.querySelector('header');
    
    // ヘッダーの高さを取得
    var headerHeight = header.offsetHeight;
    
    // ボディのpadding-topを設定
    document.body.style.paddingTop = headerHeight + 'px';
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
userIcon.addEventListener('click', () => {
    userIcon.classList.toggle('active');
});

// スクロールイベントでヘッダーを非表示
let lastScrollTop = 0;
window.addEventListener('scroll', () => {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    if (scrollTop > lastScrollTop) {
        document.querySelector('header').style.top = '-100px'; // ヘッダーを上に隠す
    } else if (scrollTop < 100) {
        document.querySelector('header').style.top = '0'; // ヘッダーを表示する
    }
    lastScrollTop = scrollTop;
});

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


/* 口コミ割引の案内モーダル処理 */

let isProcessingReviewModal = false; 


// モーダル背景と本体、起動用のリンク要素を取得
const modalBackGround = document.querySelector('.review-coupon-guide-modal-bk');
const modalContent = document.querySelector('.review-coupon-guide-modal');
const reviewCouponNavi = document.querySelector('.review-coupon-navi');

//口コミのInfoを押下するとモーダルを表示する
reviewCouponNavi.addEventListener('click', () => {
    modalBackGround.classList.add('open');
});

//モーダル以外の部分をクリックするとモーダルを閉じる
modalBackGround.addEventListener('click', function(event) {
  // モーダル本体をクリックした場合は何もしない
  if (event.target !== modalContent && !modalContent.contains(event.target)) {    
    if(isProcessingReviewModal) return;//実行中のクリックは何もしない
    isProcessingReviewModal = true;
    
    modalBackGround.classList.add('close');
    setTimeout(() => {
    	modalBackGround.classList.remove('open');
    	modalBackGround.classList.remove('close');
    	isProcessingReviewModal = false;
  	}, 500); 
  }
});










