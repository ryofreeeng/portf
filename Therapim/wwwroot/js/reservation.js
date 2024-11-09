document.addEventListener('DOMContentLoaded', function() {

/* イベント設定 */
    // コースセレクトボックスが変更されたときのイベント設定    
    document.getElementById('courseSelect').addEventListener('change', function() {
        setCourseValues();
    });

    // オプションセレクトボックスが変更されたときのイベント設定        
    document.getElementById('CourseOptionId').addEventListener('change', function() {
        setOptionValues();
    });

	// 出張か来店かによる表示の切り替えイベント設定
	document.querySelector('input[name="IsVisit"]').addEventListener('change', function() {	    
	   changeDispWhetherIsVist(this.checked);
	});

	// サロン場所チェックボックスの選択された値を hidden フィールドに設定するイベント設定
	document.querySelectorAll('.salon-checkbox').forEach(checkbox => {
	    checkbox.addEventListener('change', updateSelectedSalons);
	});

	// 開始時刻の入力時に終了時刻を自動で設定するイベント設定
	document.querySelectorAll('.datetime-start').forEach(input => {
	    input.addEventListener('change', function() {
			setEndTimeFromStartTime(this);
	    });
	});


/* 初期処理 */    
    // コースとオプションの表示名の初期値を設定
    setCourseValues();
	setOptionValues();
	// 出張希望のチェック状態により表示非表示を切り替え
	changeDispWhetherIsVist(document.querySelector('input[name="IsVisit"]').checked);
	
	// 表示時点でサロンのチェックボックス選択を復元
	recoverSelectedSalons();
	
	// 初期表示時点で第一希望の開始時刻と終了時刻を自動入力
	setStartTimeAndEndTime();
});



/* 関数定義 */
// 選択されているコースとオプションの表示名をリクエスト内容としてセットする関数
function setCourseValues() {
    // コースセレクトボックスの要素を取得
    const courseSelect = document.getElementById('courseSelect');
    const courseNameDisp = document.getElementById('courseNameDisp');
    // コースの初期値
    if (courseSelect.selectedIndex >= 0) {
        const selectedOption = courseSelect.options[courseSelect.selectedIndex];
        const selectedCourseName = selectedOption.text;
        courseNameDisp.value = selectedCourseName;
    }
}

// 選択されているオプションの表示名をリクエスト内容としてセットする関数
function setOptionValues() {
    // オプションセレクトボックスの要素を取得
    const courseOptionSelect = document.getElementById('CourseOptionId');
    const courseOptionDisp = document.getElementById('CourseOption');
    // オプションの初期値
    if (courseOptionSelect.selectedIndex >= 0) {
        const selectedOption = courseOptionSelect.options[courseOptionSelect.selectedIndex];
        const selectedCourseOption = selectedOption.text;
        courseOptionDisp.value = selectedCourseOption;
    }
}

// 出張か来店かによって入力欄の表示を切り替える関数
function changeDispWhetherIsVist(isVist){
	if (isVist) {
	    // チェックが入っている場合（出張希望）
	    visitSection.style.display = 'block';
	    salonSection.style.display = 'none';
	} else {
	    // チェックが入っていない場合（来店希望）
	    visitSection.style.display = 'none';
	    salonSection.style.display = 'block';
	}
}

// 選択されたサロンをカンマ区切りで連結してhiddenにセットする関数
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

// 希望の開始時刻が入力されたらそれに応じた終了時刻を自動入力する関数
function setEndTimeFromStartTime(startTimeElement){
	const hourSpan = 3; //この時間後に設定する
	const endFieldId = startTimeElement.getAttribute('data-pair');
	const endField = document.getElementById(endFieldId);

	if (startTimeElement.value) {
	    // 開始時刻から3時間後の時刻を計算
	    const startDate = new Date(startTimeElement.value);
	    const endDate = new Date(startDate.getTime() + (hourSpan * 60 * 60 * 1000));
	    
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
}

// 初期表示時点で第一希望の開始時刻と終了時刻を自動入力する関数
function setStartTimeAndEndTime(){
	// 開始時刻の要素を取得
    const startField = document.querySelectorAll('.datetime-start')[0];
    // 現状の開始時刻の西暦を見て、2024より小さければ初期化する
    let startDate = new Date(startField.value);
    const startYear = startDate.getFullYear();
    if(startYear < 2024){
	    // 現在時刻とその３時間後を取得
	    startDate = new Date();	
	    // 10分単位に丸める
	    startDate.setMinutes(Math.ceil(startDate.getMinutes() / 10) * 10)
	    // YYYY-MM-DDThh:mm 形式に変換
	    const startDateString = startDate.getFullYear() + '-' +
	        String(startDate.getMonth() + 1).padStart(2, '0') + '-' +
	        String(startDate.getDate()).padStart(2, '0') + 'T' +
	        String(startDate.getHours()).padStart(2, '0') + ':' +
	        String(startDate.getMinutes()).padStart(2, '0');
	    // 値をセット
	    startField.value = startDateString;
	    // 終了時刻も入力
	    setEndTimeFromStartTime(startField)
	    
	}
}

// サロンの文字列があれば、チェックボックスをチェックしておく関数(POSTBACK考慮)
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