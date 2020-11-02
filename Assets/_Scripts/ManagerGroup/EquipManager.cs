using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipManager : MonoBehaviour {
    public CurCharManager curManager;
    public GameObject Char;

    public GameObject MainUI;
    AudioSource Gift;
    public GameObject Pang;

	public int cur_button;//현재 목록 표시

	string[] chars=new string[] {"리트리버","비글","슈나우져","포메라니안","허스키"};//캐릭터 임시
    int[] charPrice = new int[] { 0, 2000, 5000, 10000, 25000 };
    string[] charAbility = new string[] { "능력 없음", "무기 공격속도 +50%", "무기 장탄수 +50%", "수레 스킬 쿨타임 -50%\n* 밟기 데미지 +2", "공격 시 무기 2번 발사\n(50% 피해)" };

    string[] wagons=new string[] {"나무수레","고급 나무수레","철제수레","펀치수레","대포수레", "얼음수레", "로케트"};//수레
    int[] wagonPrice = new int[] { 0, 1000, 3000, 5500, 8000 , 20000, 20000};
    int[] wagonSlot = new int[] { 6, 9, 13, 12, 11, 16, 15};

    string[] weapons=new string[] {"돌멩이","토마토","장난감 칼","글록","미니 미사일", "종이비행기", "상어 미사일","개 사료", "레이져 건"};//무기
    int[] weaponPrice = new int[] { 10000, 700, 1000, 1500, 3000, 4500, 7000, 6000, 11000};
    int[] weaponPrice2 = new int[] { 15000, 1500, 5000, 5500, 9000, 8000, 15000, 6000, 25000};
    int[] weaponPrice3 = new int[] { 20000, 4500, 10000, 11500, 16000, 11000, 28000, 6000, 40000};
    double[] power = new double[] { 1f, 1f, 1.5f, 2f, 1.5f, 1.5f, 2.5f, 0.8, 10f, 0, 1.5f};
    double[] knockBack = new double[] { 1, 1, 1.5, 1.5, 2 , 1, 2.5};
    double[] coolTime = new double[] { 0.2, 0.1, 0.3, 0.2, 0.3, 0.1, 0.3};

    string[] cashItems=new string[] {"광고제거","25000골드","50000골드","100000골드"};
    int[] cashItemPrice = new int[] { 2000, 4900, 8900, 14900};
    string[] cashItemAbility = new string[] { "광고를 제거합니다.", "골드+25000", "골드+50000", "골드+100000"};

	float minSwipeDist;
	Vector2 touchDownPos;
	Vector2 swipeDirection;
	Vector3 mouseDownPos;

	public Text menu;//메뉴
	public Text content; //내용
	public Text gold;//현재 골드
	public Text ability;//능력치 
	public Text but_p_text;//가격표시
    public Text but_up_text;//가격표시
    public GameObject left;//왼쪽 버튼
    public GameObject right;//오른쪽 버튼
    public ScrollRect scroll;

	public GameObject but_c;//선택
	public GameObject but_p;//구매
    public GameObject but_up;//무기 업글 버튼 

    public GameObject List;//아이템 업글 리스트
	public GameObject popUp;//팝업창
	public GameObject swiper;//스와이프될때 

    public GameObject BackBt;
    Vector3 BackBtPos;

    public GameObject Lock1;
    public GameObject Lock2;
    public GameObject Lock3;
    public Text WpAbilityText1;
    public Text WpAbilityText2;
    public Text WpAbilityText3;



	double fTime = 0;
	int count;
	int content_max;
    int check = -1; //수레넘침상황을 위한 변수
    float scrollUnit = 1.0f / 6;
    bool popUpOn = false;

    Vector2 currentSwipe;

    void Awake(){
        init();
        cur_button = 0;//초기값은 첫화면(캐릭터)
		menu.text="캐릭터";
        content_max = chars.Length;
		content.text = chars [count];
        for (int i = 0; i < content_max; i++)
            showContent(i, 1, 2, 3, 4);

        for (int i = 5; i < 9; i++)
        { // 히든아이템부분 초기화
            swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(4).gameObject.SetActive(false);

        }
        Gift = GameObject.Find("Gift").GetComponent<AudioSource>();
        BackBtPos = BackBt.transform.position;     
    }

    void init()
    {
        gold.text = "" + PlayerPrefs.GetInt("curGold");
        content.text = chars[PlayerPrefs.GetInt("curCh")];
        count = PlayerPrefs.GetInt("curCh");//착용 캐릭터
        //swiper.transform.Translate(Vector3.left * 3 * count);
        setPurchaseBtn();
        setLockScreen();

        scroll.horizontalNormalizedPosition = scrollUnit * count;

    }


	void Update () 
    {
		processInput();

            switch (cur_button) {
		case 0://캐릭터
                //if (charPrice[count] == -1 && PlayerPrefs.GetInt("p_charLock" + count) == 1) // 잠겨있는 hidden
                //    content.text = "???";
                //else
                    content.text = chars[count];
			break;
		case 1://수레
                //if (wagonPrice[count] == -1 && PlayerPrefs.GetInt("p_wgLock" + count) == 1) // 잠겨있는 hidden
               //     content.text = "???";
                //else
                    content.text = wagons[count];
			break;
		case 2://무기
                //if (weaponPrice[count] == -1 && PlayerPrefs.GetInt("p_wpLock" + count) == 1) // 잠겨있는 hidden
                //    content.text = "???";
               // else
                    content.text = weapons[count];
			break;
        case 3://캐쉬
                //if (weaponPrice[count] == -1 && PlayerPrefs.GetInt("p_wpLock" + count) == 1) // 잠겨있는 hidden
                //    content.text = "???";
                // else
                content.text = cashItems[count];
                break;
		}
      
            gold.text = ""+ PlayerPrefs.GetInt("curGold");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainUI.SetActive(true);
            gameObject.SetActive(false);
        }
           
    }


    public void WpUpgrade()
    {
        int curGold = PlayerPrefs.GetInt("curGold");
        int cur_WpLevel = PlayerPrefs.GetInt("WpLevel" + count);
        int cur_price = 0;

        if (cur_WpLevel == 0)
            cur_price = weaponPrice[count];
        if (cur_WpLevel == 1)
            cur_price = weaponPrice2[count];
        if (cur_WpLevel == 2)
            cur_price = weaponPrice3[count];

        if (curGold >= cur_price)
        {
            PlayerPrefs.SetInt("curGold", curGold - cur_price);
            PlayerPrefs.SetInt("WpLevel" + count, cur_WpLevel + 1);
            StartCoroutine("UpGradeAnim");
        }
        else
        {
            StartCoroutine("PopUpOpenAnim");
            popUp.SetActive(true);
            ability.text = "골드가 부족합니다.";
        }   
        setLockScreen();
        setPurchaseBtn();
    }

    void setPurchaseBtn() {
        switch (cur_button) {
            case 0:
                but_up.SetActive(false);
                List.SetActive(false);
                BackBt.transform.position = new Vector3(BackBtPos.x, BackBt.transform.position.y, BackBt.transform.position.z);

                if (PlayerPrefs.GetInt("p_charLock" + count) == 1)
                {//잠겨있으면
                    if (charPrice[count] == -1)
                    { // hidden 캐릭터일 때
                        but_c.SetActive(false);
                        but_p.SetActive(false);
                    }
                    else
                    {
                        but_c.SetActive(false);
                        but_p.SetActive(true);//구매버튼 활성화
                        but_p_text.text = charPrice[count] + "";
                    }
                }
                else
                {
                    but_c.SetActive(true);
                    but_p.SetActive(false);

                    if (PlayerPrefs.GetInt("curCh") == count) //사용중
                    {
                        but_c.transform.GetChild(1).gameObject.SetActive(true);
                        but_c.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        but_c.transform.GetChild(0).gameObject.SetActive(true);
                        but_c.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                break;
            case 1:
                but_up.SetActive(false);
                List.SetActive(false);
                BackBt.transform.position = new Vector3(BackBtPos.x, BackBt.transform.position.y, BackBt.transform.position.z);

                if (PlayerPrefs.GetInt("p_wgLock" + count) == 1)
                {//잠겨있으면
                    if (wagonPrice[count] == -1)
                    { // hidden 수레일 때
                        but_c.SetActive(false);
                        but_p.SetActive(false);
                    }
                    else
                    {
                        but_c.SetActive(false);
                        but_p.SetActive(true);//구매버튼 활성화
                        but_p_text.text = wagonPrice[count] + "";
                    }
                }
                else
                {
                    but_c.SetActive(true);
                    but_p.SetActive(false);
                    if (PlayerPrefs.GetInt("curWg") == count) //사용중
                    {
                        but_c.transform.GetChild(1).gameObject.SetActive(true);
                        but_c.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        but_c.transform.GetChild(0).gameObject.SetActive(true);
                        but_c.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                break;
            case 2:
                but_c.SetActive(false);
                but_p.SetActive(false);
                but_up.SetActive(true);
                List.SetActive(true);

                if (PlayerPrefs.GetInt("ad") == 0)
                    BackBt.transform.position = new Vector3(BackBtPos.x + 5, BackBt.transform.position.y, BackBt.transform.position.z);
                else
                    BackBt.transform.position = new Vector3(BackBtPos.x, BackBt.transform.position.y, BackBt.transform.position.z);


                int WpLevel = PlayerPrefs.GetInt("WpLevel" + count);
          
                if (WpLevel == 0)
                {
                    but_up_text.text = weaponPrice[count] + "";
                    Lock1.SetActive(true);
                    Lock2.SetActive(true);
                    Lock3.SetActive(true);
                    WpAbilityText1.text = "";    
                    WpAbilityText2.text = "";    
                    WpAbilityText3.text = "";    
                }
                if (WpLevel == 1)
                {
                    but_up_text.text = weaponPrice2[count] + "";
                    Lock1.SetActive(false);
                    Lock2.SetActive(true);
                    Lock3.SetActive(true);  
                    WpAbilityText2.text = "";    
                    WpAbilityText3.text = ""; 
                }
                if (WpLevel == 2)
                {
                    but_up_text.text = weaponPrice3[count] + "";
                    Lock1.SetActive(false);
                    Lock2.SetActive(false);
                    Lock3.SetActive(true);  
                    WpAbilityText3.text = ""; 
                }
                if (WpLevel == 3)
                {
                    Lock1.SetActive(false);
                    Lock2.SetActive(false);
                    Lock3.SetActive(false);
                    but_up.SetActive(false);           
                }
                if (count == 0) // 돌멩이
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "돌멩이 크기 증가, 공격력 +0.5";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "돌멩이 속도 증가";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "명중 시 스플래시 발생";
                }
                if (count == 1) // 토마토
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "토마토 발사거리 증가";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "토마토 넉백률 증가";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "토마토 크기상승, 공격력 +0.5";
                }
                if (count == 2) // 장난감칼
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "장난감 칼 크기 증가, 넉백률 증가";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "장난감 칼 공격력 +0.5";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "공격 시 10% 확률로 엑스칼리버 소환";
                }
                if (count == 3) // 글록
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "장탄수 +4";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "장전속도 증가";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "공격 시 스플래시 폭발 발생";
                }
                if (count == 4) // 미니미사일
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "미사일 공격력 +0.5";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "미사일 크기 증가";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "미사일 장탄수 +10";
                }
                if (count == 5) // 종이비행기
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "넉백률 증가";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "장탄수 +30";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "종이비행기 대신 RC비행기 발사"; // 미완
                }
                if (count == 6) // 상어미사일
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "폭발 범위 증가";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "장탄수 +10";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "공격 시 2연발 발사";
                }
                if (count == 7) // 개사료
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "공격력 +0.5";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "공격력 +0.5";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "공격력 +0.5";
                }
                if (count == 8) // 레이져
                {
                    if (WpLevel >= 1)
                        WpAbilityText1.text = "장탄수 +5";                
                    if (WpLevel >= 2)
                        WpAbilityText2.text = "레이져 크기 상승";
                    if (WpLevel >= 3)
                        WpAbilityText3.text = "공격 시 전체 적에게 새틀라이트 빔";
                }
                break;
            case 3:
                but_up.SetActive(false);
                List.SetActive(false);
                BackBt.transform.position = new Vector3(BackBtPos.x, BackBt.transform.position.y, BackBt.transform.position.z);
                but_c.SetActive(false);
                but_p.SetActive(true);
                but_p_text.text = cashItemPrice[count] + "";
                break;

                /*
                if (PlayerPrefs.GetInt("p_wpLock" + count) == 1)
                {//잠겨있으면
                    if (weaponPrice[count] == -1)
                    { // hidden 무기일 때
                        but_c.SetActive(false);
                        but_p.SetActive(false);
                    }
                    else
                    {
                        but_c.SetActive(false);
                        but_p.SetActive(true);//구매버튼 활성화
                        but_p_text.text = weaponPrice[count] + "";
                    }
                }
                else
                {
                    but_c.SetActive(true);
                    but_p.SetActive(false);
                    if (PlayerPrefs.GetInt("curWp") == count) //사용중
                    {
                        but_c.transform.GetChild(1).gameObject.SetActive(true);
                        but_c.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        but_c.transform.GetChild(0).gameObject.SetActive(true);
                        but_c.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                */
                break;
        }
        
    }

    void setLockScreen() { // 항목아이템만 보여주고 잠금 가려주기
        switch (cur_button) {
            case 0:
                for (int i = 0; i < content_max; i++)
                {
                    showContent(i, 1, 2, 3, 4);
                    if (PlayerPrefs.GetInt("p_charLock" + i) == 0)
                    {//열림
                        swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false); 
                    }
                    else
                    {
                        swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                        shadowOnOff(i, 0, 1, 2);
                        swiper.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                    }

                }
                break;
            case 1:
                for (int i = 0; i < content_max; i++)
                {
                    showContent(i, 2, 1, 3, 4);
                    if (PlayerPrefs.GetInt("p_wgLock" + i) == 0)
                    {//열림
                        swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                        shadowOnOff(i, 1, 0, 2);
                        swiper.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
                    }

                }
                break;
            case 2:
                for (int i = 0; i < content_max ; i++)
                {
                    showContent(i, 3, 1, 2, 4);
                    if (PlayerPrefs.GetInt("p_wpLock" + i) == 0)
                    {//열림
                        swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                        //shadowOnOff(i, 2, 0, 1);
                        //swiper.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
                    }

                }
                break;
            case 3:
                for (int i = 0; i < content_max ; i++)
                {
                    showContent(i, 4, 1, 2, 3);
                    swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }

                break;

        }
      
    }

    void shadowOnOff(int i, int on, int off1, int off2) {
        swiper.transform.GetChild(i).GetChild(0).GetChild(on).gameObject.SetActive(true);
        swiper.transform.GetChild(i).GetChild(0).GetChild(off1).gameObject.SetActive(false);
        swiper.transform.GetChild(i).GetChild(0).GetChild(off2).gameObject.SetActive(false);

    }

	void OnEnable(){

        for (int i = 5; i < 9; i++)
        { // 히든아이템부분 초기화
            swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(4).gameObject.SetActive(false);

        }
        menu.text="캐릭터";
            content_max = chars.Length;
            content.text = chars[PlayerPrefs.GetInt("curCh")] ;
            cur_button = 0;
            count = PlayerPrefs.GetInt("curCh");
            scroll.horizontalNormalizedPosition = scrollUnit * count;

        popUp.SetActive(false);
        setPurchaseBtn();
            setLockScreen();
	}

    //====== function =======//

    public void but_choice(){ //목록을 버튼으로 선택하면 cur_button을 변경하는 함수
		string name = EventSystem.current.currentSelectedGameObject.name;
        check = -1;
        if (name.Equals("right"))
        {
            cur_button++;
            cur_button = cur_button % 2;
        }
        else if (name.Equals("left"))
        {
            if (cur_button == 0)
            {
                cur_button = 1;
            }
            else {
                cur_button--;
            }

        }

        for (int i = 5; i < 7; i++) { // 히든아이템부분 초기화
            swiper.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(2).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(3).gameObject.SetActive(false);
            swiper.transform.GetChild(i).GetChild(4).gameObject.SetActive(false);

        }

        switch (cur_button) {
		case 0:
            menu.text = "캐릭터";
            content_max = chars.Length;
            count = PlayerPrefs.GetInt("curCh");
			break;
		case 1:
                menu.text = "수레";
                content_max = wagons.Length;
                count = PlayerPrefs.GetInt("curWg");
			break;

		case 2:
			menu.text = "무기";
                content_max = weapons.Length;
                count = PlayerPrefs.GetInt("curWp");
			break;
        case 3:
                menu.text = "금은방";
                content_max = cashItems.Length;
                count = 0;
                break;
		}
        scroll.horizontalNormalizedPosition = scrollUnit * count;
        setPurchaseBtn();
        setLockScreen();
	}
    public void showContent( int  turn ,int show, int blind1, int bline2 , int bline3) {
        swiper.transform.GetChild(turn).GetChild(show).gameObject.SetActive(true);
        swiper.transform.GetChild(turn).GetChild(blind1).gameObject.SetActive(false);
        swiper.transform.GetChild(turn).GetChild(bline2).gameObject.SetActive(false);
        swiper.transform.GetChild(turn).GetChild(bline3).gameObject.SetActive(false);

    }

	public void equip_choice(){//선택버튼 눌렀을때
		switch (cur_button) {
		case 0://캐릭터
               //NowStateData의 캐릭터ID 변경
                PlayerPrefs.SetInt("curCh", count);
            GameObject.Find("curChar").SendMessage("changeChar"); //이미지 바꾸기
            break;
		case 1://수레
               //NowStateData의 수레ID 변경
                /*
                if (wagonSlot[count] < PlayerPrefs.GetInt("curMilkNum") && check!=count) 
                { //새로 선택한 수레크기 보유 우유개수보다작으면
                    StartCoroutine("PopUpOpenAnim");
					popUp.SetActive (true);
                    ability.text = "가지고 있는 우유를\n모두 넣을 수 없어요!\n(넘치는 우유 "+((PlayerPrefs.GetInt("curMilkNum"))-(wagonSlot[count]))+"개가 사라집니다)";
					check = count; //check를 해당 번호로 바꿔서 다시 선택하면 허용해준다.
                } 
                else 
                { //아니면
    				if (check == count) 
                    {                     //우유를 버려야 할때
                        popUp.SetActive(false);
    					for (int i = wagonSlot[count]; i < PlayerPrefs.GetInt("curMilkNum"); i++) 
                            PlayerPrefs.SetInt("p_wgContent" + i, -1); //오른쪽부터 비워주기
    					
                        PlayerPrefs.SetInt("curMilkNum", wagonSlot[count]); //빼버리기
    					check = -1;
    				}
                    PlayerPrefs.SetInt("curWg", count);
                    GameObject.Find("curChar").SendMessage("changeWag"); //이미지 바꾸기
                }
                */
                PlayerPrefs.SetInt("curWg", count);
                PlayerPrefs.SetInt("curMilkNum", wagonSlot[PlayerPrefs.GetInt("curWg")]);
                GameObject.Find("curChar").SendMessage("changeWag"); //이미지 바꾸기
			break;
		case 2://무기
               //NowStateData의 무기ID 변경
                PlayerPrefs.SetInt("curWp", count);
			break;
		}
        PlayerPrefs.Save();
        setPurchaseBtn();
	}

	public void PopUp(int num){//눌렀을 때 팝업창
        if (popUpOn == false)
        {
            popUpOn = true;
            StartCoroutine("PopUpOpenAnim");
            //if (PlayerPrefs.GetInt("FirstPop") != 1)
             //  PlayerPrefs.SetInt("FirstPop", 1);
            
        }
            ability.text = "";
            switch (cur_button)
            {
                case 0://캐릭터
                   // if (charPrice[num] == -1 && PlayerPrefs.GetInt("p_charLock" + num) == 1) // 잠겨있는 hidden
                   //     ability.text = "확인불가";
                   // else
                    //{
                        if (num == 0)
                            ability.text = "* " + charAbility[num] + "\n\n귀여운 골든 리트리버다.";
                        if (num == 1)
                        ability.text = "* " + charAbility[num] + "\n\n활발한 비글이다.";
                        if (num == 2)
                            ability.text = "* " + charAbility[num] + "\n\n잘생긴 슈나우져다.";
                        if (num == 3)
                            ability.text = "* " + charAbility[num] + "\n\n애교 넘치는 포메라니안이다.";
                        if (num == 4)
                            ability.text = "* " + charAbility[num] + "\n\n허숙 허숙 허숙희";
                   // }
                    break;
                case 1://수레
                   // if (wagonPrice[num] == -1 && PlayerPrefs.GetInt("p_wgLock" + num) == 1) // 잠겨있는 hidden
                   //     ability.text = "확인불가";
                   // else
                   // {
                        if (num == 0)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n기본적인 나무 수레이다.";
                        if (num == 1)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n고급 목재로 만든 수레이다.";
                        if (num == 2)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n튼튼한 철로 만든 수레이다.";
                        if (num == 3)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n- 다가오는 적에게 펀치를 날려 밀어낸다.";
                        if (num == 4)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n- 일정 간격으로 포탄을 발사한다.";
                        if (num == 5)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n- 돌멩이 대신 얼음덩어리를 발사한다.";
                        if (num == 6)
                            ability.text = "* 우유 적재량 : " + wagonSlot[num] + "\n\n- 3단 점프 가능\n- 부스터 지속시간 +2초";
                   // }
                    break;
                case 2://무기
                   // if (weaponPrice[num] == -1 && PlayerPrefs.GetInt("p_wpLock" + num) == 1) // 잠겨있는 hidden
                   //     ability.text = "확인불가";
                   // else
                   // {
                        if (num == 0)
                            ability.text = "* 공격력 : " + power[num] + "\n\n직선으로 날아가는 돌멩이다.";
                        if (num == 1)
                            ability.text = "* 공격력 : " + power[num] + "\n\n곡선으로 날아가고 연사속도가 빠르다.";
                        if (num == 2)
                            ability.text = "* 공격력 : " + power[num] + "\n\n둥근 플라스틱 장난감칼이다.";
                        if (num == 3)
                            ability.text = "* 공격력 : " + power[num] + "\n\n8발 발사/장전이 가능하다.";
                        if (num == 4)
                            ability.text = "* 공격력 : " + power[num] + "\n\n폭발은 주변 적들도 피해를 입는다.";
                        if (num == 5)
                            ability.text = "* 공격력 : " + power[num] + "\n\n적들을 관통할 수 있다.";
                        if (num == 6)
                            ability.text = "* 공격력 : " + power[num] + "\n\n폭발은 주변 적들도 피해를 입는다.";
                        if (num == 7)
                            ability.text = "* 공격력 : " + power[num] + "\n\n연사속도가 빠르지만 데미지가 낮다.";
                        if (num == 8)
                            ability.text = "* 공격력 : " + power[num] + "\n\n매우 강력한 레이져다.";
                
                   // }

                    break;
            case 3://캐쉬
                // if (charPrice[num] == -1 && PlayerPrefs.GetInt("p_charLock" + num) == 1) // 잠겨있는 hidden
                //     ability.text = "확인불가";
                // else
                //{
                if (num == 0)
                    ability.text = "* " + cashItemAbility[num];
                if (num == 1)
                    ability.text = "* " + cashItemAbility[num];
                if (num == 2)
                    ability.text = "* " + cashItemAbility[num];
                if (num == 3)
                    ability.text = "* " + cashItemAbility[num];
                // }
                break;
            }
        
	}
    public void PopUpClose()
    {
        popUpOn = false;
        StartCoroutine("PopUpCloseAnim");
    }

	public void purchase(){// 구매버튼 눌렀을때
        int curGold = PlayerPrefs.GetInt("curGold");
        switch (cur_button)
        {
            case 0: // 캐릭터
                if (curGold >= charPrice[count])
                {
                    PlayerPrefs.SetInt("curGold", curGold - charPrice[count]); //돈 가져가고
                    PlayerPrefs.SetInt("p_charLock" + count, 0); //잠금해제
                    but_c.SetActive(true);//선택 활성화
                }
                else {
                    StartCoroutine("PopUpOpenAnim");
                    popUp.SetActive(true);
                    ability.text = "골드가 부족합니다.";
                }
                break;
            case 1: // 수레
                if (curGold >= wagonPrice[count])
                {
                    PlayerPrefs.SetInt("curGold", curGold - wagonPrice[count]); //돈 가져가고
                    PlayerPrefs.SetInt("p_wgLock" + count, 0); //잠금해제  
                }
                else
                {
                    StartCoroutine("PopUpOpenAnim");
                    popUp.SetActive(true);
                    ability.text = "골드가 부족합니다.";
                }
                break;
            case 2: // 무기
                if (curGold >= weaponPrice[count])
                {
                    PlayerPrefs.SetInt("curGold", curGold - weaponPrice[count]); //돈 가져가고
                    PlayerPrefs.SetInt("p_wpLock" + count, 0); //잠금해제   
                }
                else
                {
                    StartCoroutine("PopUpOpenAnim");
                    popUp.SetActive(true);
                    ability.text = "골드가 부족합니다.";
                }
                break;
            case 3: // 캐쉬
                if (curGold >= cashItemPrice[count])
                {
                    PlayerPrefs.SetInt("curGold", curGold - cashItemPrice[count]); //돈 가져가고
                    //인앱결제 코드
                }
                else {
                    StartCoroutine("PopUpOpenAnim");
                    popUp.SetActive(true);
                    ability.text = "골드가 부족합니다.";
                }
                break;
        }
        PlayerPrefs.Save();
        setLockScreen();
        setPurchaseBtn();
        // 저~장
        //PlayerPrefs.Save();
    }

	//====== 스와이프 =======//

	bool swiped = false;

	void processInput(){
		if (Input.GetMouseButtonDown(0)) {
            mouseDownPos = Input.mousePosition;
			swiped = false;
		}
		else if(Input.GetMouseButton(0)){
			int swipeDetected = checkSwipe (mouseDownPos, Input.mousePosition);
			if (Input.mousePosition.y < Screen.height/2|| mouseDownPos.y< Screen.height/2 || Input.mousePosition.y > (Screen.height/5)*4|| mouseDownPos.y> (Screen.height/5)*4) {
				//Debug.Log ("범위 이탈"); //범위이탈 범위 수정부분
				return;
			}
			swipeDirection = (Input.mousePosition - mouseDownPos).normalized;
            switch (swipeDetected) {
                case 1://왼쪽 한번
                    onSwipeDetectedLeft(swipeDirection);
                    break;
                case 2://오른쪽 한번
                    onSwipeDetectedRight(swipeDirection);
                    break;
            }
            //string l_name;
            setPurchaseBtn();
		}

		else if(Input.GetMouseButtonUp(0)){
			//Debug.Log ("스와이프 거리: "+swipeDirection);
			swiped = false;
		}
	}

	int checkSwipe(Vector3 downPos, Vector3 currentPos){
		if (swiped == true)
			return 0;

		if (isInputBlocked == true)
			return 0;

		currentSwipe = currentPos - downPos;
        if (-currentSwipe.x >= 20) return 1;
        else if (currentSwipe.x >= 20) return 2;
        //Debug.Log(currentSwipe.x);
        return 5;
	}
		
	void onSwipeDetectedLeft(Vector2 swipeDirecrion){
		//Swipe
		if (count < content_max-1) {
			//Debug.Log ("L스와이프됨");
			count++;
            StartCoroutine("moveR");
        }

        swiped = true;
	}
	void onSwipeDetectedRight(Vector2 swipeDirecrionm){
        //Swipe
        if (count > 0)
        {
            //Debug.Log("R스와이프됨");
            count--;
            StartCoroutine("moveL");
        }
		swiped = true;
	}

    void moveScroll() // 여러개 이동
    {
        while (true) {
            if (Input.GetTouch(0).deltaPosition.x >= 0)
            {
                int scrollCount = (int)((Screen.width - Input.GetTouch(0).deltaPosition.x) % 5);
                if (count + scrollCount < 5)
                { // 정상범위
                    for (int i = 0; i < scrollCount; i++)
                    {
                        count++;
                        StartCoroutine("moveR");
                    }
                }
            }
            else {
                int scrollCount = (int)((Screen.width + Input.GetTouch(0).deltaPosition.x) % 5);
                if (count - scrollCount >= 0)
                { // 정상범위
                    for (int i = 0; i < scrollCount; i++)
                    {
                        count--;
                        StartCoroutine("moveL");
                    }
                }
            }
        }
    }

    IEnumerator moveR() // 우로
    {
        float num = (float)count - 1.0f;
        for (int i = 0; i < 10; i++) {
            num += 1.0f / 10;
            scroll.horizontalNormalizedPosition = scrollUnit * num;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator moveL() // 좌로
    {
        float num = (float)count + 1.0f;
        for (int i = 0; i < 10; i++){
            num -= 1.0f / 10;
            scroll.horizontalNormalizedPosition = scrollUnit * num;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private bool isInputBlocked = false;
	public void blockInput(){
		isInputBlocked = true;
	}
	public void unBlockInput(){
		isInputBlocked = false;
	}

    IEnumerator PopUpOpenAnim()
    {
        popUp.transform.localScale = new Vector3(0f, 0f, 0);
        int time = 0;
        while (time < 6)
        {
            popUp.transform.localScale += new Vector3(0.5f, 0.5f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
        while (time < 5)
        {
            popUp.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
    }
    IEnumerator PopUpCloseAnim()
    {
        int time = 0;
        while (time < 5)
        {
            popUp.transform.localScale -= new Vector3(0.5f, 0.5f, 0);
            yield return new WaitForSeconds(0.001f);
            time++;
        }
        time = 0;
    }
    IEnumerator UpGradeAnim()
    {
        GameObject Weapon = swiper.transform.GetChild(count).GetChild(3).gameObject;
        if (PlayerPrefs.GetInt("music") == 1)
            Gift.Play();
        
        for (int i = 0; i < 25; i++)
            Instantiate(Pang, Weapon.transform.position, Weapon.transform.rotation);

        int time = 0;
        while (time < 3)
        {
            Weapon.transform.localScale -= new Vector3(0.0005f, 0.0005f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        while (time < 3)
        {
            Weapon.transform.localScale += new Vector3(0.001f, 0.001f, 0);
            yield return new WaitForSeconds(0.005f);
            time++;
        }
        time = 0;
        Weapon.transform.localScale = new Vector3(0.005f, 0.005f, 1);

        yield return null;
    }

    public void DogEquip()
    {
        cur_button = 0;
        but_choice();
    }
    public void WagonEquip()
    {
        cur_button = 1;
        but_choice();
    } 
    public void WeaponEquip()
    {
        cur_button = 2;
        but_choice();
    }
    public void CashEquip()
    {
        cur_button = 3;
        but_choice();
    }
}
