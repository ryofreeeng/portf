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