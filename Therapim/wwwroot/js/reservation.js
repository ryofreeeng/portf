document.addEventListener('DOMContentLoaded', function() {
	
	/* 表示時点で出張フラグをチェックして表示切替（POSTBACKを考慮） */
	const visitSection = document.getElementById('visitSection');
	const salonSection = document.getElementById('salonSection');
	const isVisit = document.querySelector('input[name="IsVisit"]');

    if (isVisit.checked) {
        // チェックが入っている場合（出張希望）
        visitSection.style.display = 'block';
        salonSection.style.display = 'none';
    } else {
        // チェックが入っていない場合（来店希望）
        visitSection.style.display = 'none';
        salonSection.style.display = 'block';
    }

	/* 表示時点でサロンのチェックボックス選択を復元(POSTBACK考慮) */
	recoverSelectedSalons();


	/* 出張か来店かによる表示の切り替えイベント設定 */
	isVisit.addEventListener('change', function() {	    
	    if (this.checked) {
	        // チェックが入っている場合（出張希望）
	        visitSection.style.display = 'block';
	        salonSection.style.display = 'none';
	    } else {
	        // チェックが入っていない場合（来店希望）
	        visitSection.style.display = 'none';
	        salonSection.style.display = 'block';
	    }
	});

	/* サロン場所チェックボックスの選択された値を hidden フィールドに設定するイベント設定 */
	document.querySelectorAll('.salon-checkbox').forEach(checkbox => {
	    checkbox.addEventListener('change', updateSelectedSalons);
	});

	/* 初期表示時点で第一希望の開始時刻と終了時刻を自動入力 */    
    const startField = document.querySelectorAll('.datetime-start')[0];
    const endFieldId = startField.getAttribute('data-pair');
	const endField = document.getElementById(endFieldId);
	        
    if(startField){
	    // 現在時刻とその３時間後を取得
	    const startDate = new Date();
	    const endDate = new Date(startDate.getTime() + (3 * 60 * 60 * 1000));
	    
	    // 10分単位に丸める
	    startDate.setMinutes(Math.ceil(startDate.getMinutes() / 10) * 10);
	    endDate.setMinutes(Math.ceil(startDate.getMinutes() / 10) * 10);
	    
	    // YYYY-MM-DDThh:mm 形式に変換
	    const startDateString = startDate.getFullYear() + '-' +
	        String(startDate.getMonth() + 1).padStart(2, '0') + '-' +
	        String(startDate.getDate()).padStart(2, '0') + 'T' +
	        String(startDate.getHours()).padStart(2, '0') + ':' +
	        String(startDate.getMinutes()).padStart(2, '0');
        const endDateString = endDate.getFullYear() + '-' +
            String(endDate.getMonth() + 1).padStart(2, '0') + '-' +
            String(endDate.getDate()).padStart(2, '0') + 'T' +
            String(endDate.getHours()).padStart(2, '0') + ':' +
            String(endDate.getMinutes()).padStart(2, '0');

	    // 値をセット
	    startField.value = startDateString;
	    endField.value = endDateString;
	}
	
	/* 開始時刻の入力時に終了時刻を自動で設定する処理イベント設定 */
	document.querySelectorAll('.datetime-start').forEach(input => {
	    input.addEventListener('change', function() {
	        const endFieldId = this.getAttribute('data-pair');
	        const endField = document.getElementById(endFieldId);
	        
	        if (this.value) {
	            // 開始時刻から3時間後の時刻を計算
	            const startDate = new Date(this.value);
	            const endDate = new Date(startDate.getTime() + (3 * 60 * 60 * 1000));
	            
	            // 10分単位に丸める
	            endDate.setMinutes(Math.ceil(endDate.getMinutes() / 10) * 10);
	            
	            // YYYY-MM-DDThh:mm 形式に変換
	            const endDateString = endDate.getFullYear() + '-' +
	                String(endDate.getMonth() + 1).padStart(2, '0') + '-' +
	                String(endDate.getDate()).padStart(2, '0') + 'T' +
	                String(endDate.getHours()).padStart(2, '0') + ':' +
	                String(endDate.getMinutes()).padStart(2, '0');
	            
	            endField.value = endDateString;
	        } else {
	            endField.value = '';
	        }
	    });
	});

	/* 選択したコースの表示名もリクエスト値に含めるための処理 */

    // コースセレクトボックスの要素を取得
    const courseSelect = document.getElementById('courseSelect');
    const courseNameDisp = document.getElementById('courseNameDisp');

    // オプションセレクトボックスの要素を取得
    const courseOptionSelect = document.getElementById('CourseOptionId');
    const courseOptionDisp = document.getElementById('CourseOption');

    // 初期値の設定
    function setInitialValues() {
        // コースの初期値
        if (courseSelect.selectedIndex >= 0) {
            const selectedOption = courseSelect.options[courseSelect.selectedIndex];
            const selectedCourseName = selectedOption.text;
            courseNameDisp.value = selectedCourseName;
        }

        // オプションの初期値
        if (courseOptionSelect.selectedIndex >= 0) {
            const selectedOption = courseOptionSelect.options[courseOptionSelect.selectedIndex];
            const selectedCourseOption = selectedOption.text;
            courseOptionDisp.value = selectedCourseOption;
        }
    }

    // 初期値を設定
    setInitialValues();

    // コースセレクトボックスが変更されたときのイベント設定
    courseSelect.addEventListener('change', function() {
        const selectedOption = courseSelect.options[courseSelect.selectedIndex];
        const selectedCourseName = selectedOption.text;
        courseNameDisp.value = selectedCourseName;
    });

    // オプションセレクトボックスが変更されたときのイベント設定
    courseOptionSelect.addEventListener('change', function() {
        const selectedOption = courseOptionSelect.options[courseOptionSelect.selectedIndex];
        const selectedCourseOption = selectedOption.text;
        courseOptionDisp.value = selectedCourseOption;
    });
});

/* 選択されたサロンをカンマ区切りで連結してhiddenにセットする処理関数 */
function updateSelectedSalons() {
    // 選択されているチェックボックスの値を取得
    const selectedSalons = Array.from(document.querySelectorAll('.salon-checkbox:checked'))
        .map(cb => cb.value)
        .join(',');
    
    // hidden フィールドに値を設定
    document.getElementById('DesiredSalon').value = selectedSalons;
    
    // バリデーション用に、少なくとも1つ選択されているかチェック
    if (selectedSalons === '') {
        // エラーメッセージを表示する処理をここに追加
    } else {
        // エラーメッセージを消す処理をここに追加
    }
}

/* サロンの文字列があれば、チェックボックスをチェックしておく(POSTBACK考慮) */
function recoverSelectedSalons() {
	const desiredSalon = document.getElementById('DesiredSalon').value; // desiredSalonの値を取得
	const selectedSalons = desiredSalon.split(","); // カンマで分割して配列に変換

	// すべてのチェックボックスを取得
	const checkboxes = document.querySelectorAll('.salon-checkbox');

	// 各チェックボックスに対してチェックを設定
	checkboxes.forEach(checkbox => {
	  // チェックボックスのvalue属性がselectedSalonsに含まれているか確認
	  if (selectedSalons.includes(checkbox.value)) {
	    checkbox.checked = true; // 含まれていればチェックを付ける
	  } else {
	    checkbox.checked = false; // 含まれていなければチェックを外す
	  }
	});
}


