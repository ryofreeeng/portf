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

// 希望の開始日時が入力されたらそれに応じた終了日時を自動入力する関数
function setEndTimeFromStartTime(startField){

	// 現在日時より前を選択したときはアラートを出して終了
	const initResult = initStartDateTime(startField);
	if(initResult == 1){
		alert("現在日時より後の日時をご選択ください");
	}
	
	const hourSpan = 3; //この時間後に設定する
	const endFieldId = startField.getAttribute('data-pair');
	const endField = document.getElementById(endFieldId);
	if (startField.value) {
	    // 終了日時は開始日時から3時間後の日時を計算
	    const startDateTime = new Date(startField.value);
	    const endDateTime = new Date(startDateTime.getTime() + (hourSpan * 60 * 60 * 1000));
	    
	    // 10分単位に丸める
	    startDateTime.setMinutes(Math.ceil(startDateTime.getMinutes() / 10) * 10);
	    endDateTime.setMinutes(Math.ceil(endDateTime.getMinutes() / 10) * 10);
	    
	    // YYYY-MM-DDThh:mm 形式に変換
	    const startDateString = startDateTime.getFullYear() + '-' +
	        String(startDateTime.getMonth() + 1).padStart(2, '0') + '-' +
	        String(startDateTime.getDate()).padStart(2, '0') + 'T' +
	        String(startDateTime.getHours()).padStart(2, '0') + ':' +
	        String(startDateTime.getMinutes()).padStart(2, '0');
	    const endDateString = endDateTime.getFullYear() + '-' +
	        String(endDateTime.getMonth() + 1).padStart(2, '0') + '-' +
	        String(endDateTime.getDate()).padStart(2, '0') + 'T' +
	        String(endDateTime.getHours()).padStart(2, '0') + ':' +
	        String(endDateTime.getMinutes()).padStart(2, '0');
	    startField.value = startDateString;
	    endField.value = endDateString;
	} else {
	    endField.value = '';
	}
}

// 初期表示時点で第一希望の開始日時と終了日時を自動入力する関数
function setStartTimeAndEndTime(){
	// 開始日時の要素を取得
    const startField = document.querySelectorAll('.datetime-start')[0];
    // 開始日時の初期化処理
    const initResult = initStartDateTime(startField);
	if(initResult >= 0){
	    // 開始日時を10分刻みに変換して、終了日時を自動入力する
	    setEndTimeFromStartTime(startField)
	}
}

// 受け取った要素の入力値が正しく日付に変換でき、現在日時より前の場合は現在日時に修正する
function initStartDateTime(startField){
	// 初期表示のときは実行だけして0を返す
    if(startField.value == ""){
		// 現在日時の1分後を取得(現在日時だと、11:19:59に実行した場合、11:20:00がセットされて、この後の比較で11:20:01などよりも前の判定になってしまうため)
		let startDateTime = new Date();
		startDateTime.setMinutes(startDateTime.getMinutes() + 1);
		
	    // 10分単位に丸める
	    startDateTime.setMinutes(Math.ceil(startDateTime.getMinutes() / 10) * 10);   
	    	    
	    // YYYY-MM-DDThh:mm 形式に変換
	    let startDateString = startDateTime.getFullYear() + '-' +
	        String(startDateTime.getMonth() + 1).padStart(2, '0') + '-' +
	        String(startDateTime.getDate()).padStart(2, '0') + 'T' +
	        String(startDateTime.getHours()).padStart(2, '0') + ':' +
	        String(startDateTime.getMinutes()).padStart(2, '0');
	        
	    // 現在日時で開始日時を上書きする
	    startField.value = startDateString;
		
		return 0;
    }
    // 古い日時がある場合は実行して１を返す
    if(new Date(startField.value) < new Date()){
		// 現在日時の1分後を取得(現在日時だと、11:19:59に実行した場合、11:20:00がセットされて、この後の比較で11:20:01などよりも前の判定になってしまうため)
		let startDateTime = new Date();
		startDateTime.setMinutes(startDateTime.getMinutes() + 1);

		// 10分単位に丸める
		startDateTime.setMinutes(Math.ceil(startDateTime.getMinutes() / 10) * 10);	    
			    
		// YYYY-MM-DDThh:mm 形式に変換
		let startDateString = startDateTime.getFullYear() + '-' +
		    String(startDateTime.getMonth() + 1).padStart(2, '0') + '-' +
		    String(startDateTime.getDate()).padStart(2, '0') + 'T' +
		    String(startDateTime.getHours()).padStart(2, '0') + ':' +
	        String(startDateTime.getMinutes()).padStart(2, '0');
		    
		// 現在日時で開始日時を上書きする
		startField.value = startDateString;

		return 1;
    }

	// それ以外の場合は何もしないで-1を返す
	return -1;
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