using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;


namespace KoganeUnityLib
{
	/// <summary>
	/// TextMesh Pro で 1 文字ずつ表示する演出を再生するコンポーネント
	/// </summary>
	[RequireComponent( typeof( TMP_Text ) )]
	public partial class TMP_Typewriter : MonoBehaviour
	{
		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		[SerializeField] private TMP_Text m_textUI = null;

		//==============================================================================
		// 変数
		//==============================================================================
		private string m_parsedText;
		private Action m_onComplete;
		private Tween m_tween;

		int textcount = 0;

		bool textcheck = false;

		bool textnextswitch = false;

		bool[] checks = new bool[5];

		[SerializeField]
		TextMeshProUGUI Rtext;

		AudioSource _audio;

		[SerializeField]
		AudioClip SE;

		[SerializeField]
		GameObject particle_;

		[SerializeField]
		GameObject Player;

		[SerializeField]
		AudioClip startSE;

		Rigidbody rb;

		float speed = 0f;

		bool changeswitch = false;

		[SerializeField]
		GameObject setumeitext;

		//==============================================================================
		// 関数

		private void Start()
		{
			rb = Player.GetComponent<Rigidbody>();

			particle_.SetActive(false);

			_audio = GetComponent<AudioSource>();
			for (int a = 0; a < checks.Length; a++)
			{
				checks[a] = false;
			}

			StartCoroutine("Coroutine1");

			_audio.PlayOneShot(SE);
			Play(text: "クリスマスの時期になると現れるアナザーヴィーナスフォート。そこでは、雪景色のヴィーナスフォートの中でサンタと陽気な仲間たちが、クリスマスの準備をしています。", speed: 10, onComplete: () => textnextswitch = true);
		}

		public void Pointdown()
        {

			if (textnextswitch)
			{


			    if (textcount == 4)
				{
					//setumeitext.SetActive(false);

					StartCoroutine("Coroutine1");

					_audio.PlayOneShot(startSE);
					particle_.SetActive(true);
					changeswitch = true;

					StartCoroutine("Coroutine2");
				}

				textcount++;
				textnextswitch = false;
			}
		}



		public void Pointenter()
        {
			if (!textnextswitch)
			{
				Rtext.text = "「A」ボタンでスキップ";
			}

			if (textnextswitch)
			{
				Rtext.text = "「A」ボタンで次へ";
			}

			if (textcount == 4)
			{
				Rtext.text = "「A」ボタンで出発する";
			}
			
		}




		IEnumerator Coroutine1()
		{
			yield return new WaitForSeconds(2f);
		}

		IEnumerator Coroutine2()
        {
			yield return new WaitForSeconds(3f);
			scenecange();
		}


		private void Update()
        {
			if(changeswitch)
            {
				if (speed < 3)
				{
					speed += 1f;
				}
			}

			if (!checks[0] && textcount == 1)
			{
				_audio.PlayOneShot(SE);
				checks[0] = true;
				Play(text: "しかし、今年のサンタはあわてんぼうのようです。集めたプレゼントを至る所で落としてしまいました。", speed: 10, onComplete: () => textnextswitch = true);
			}
			if (!checks[1] && textcount == 2)
			{
				_audio.PlayOneShot(SE);
				checks[1] = true;
				Play(text: "そこであなたにお願いがあります。サンタのもとに、落としたプレゼントを集めて、教会広場に届けてくれませんか？", speed: 10, onComplete: () => textnextswitch = true);
			}
			if (!checks[3] && textcount == 3)
			{
				_audio.PlayOneShot(SE);
				checks[3] = true;
				Play(text: "ただし、あなたがその不思議な世界に入り込むには、サンタ見習いになって、カラダを小さくする必要があります。", speed: 10, onComplete: () => textnextswitch = true);
			}
			if (!checks[4] && textcount == 4)
			{
				_audio.PlayOneShot(SE);
				checks[4] = true;
				Play(text: "VRゴーグルを身につけてアナザーヴィーナスフォートに出発しましょう。", speed: 10, onComplete: () => textnextswitch = true);
			}


			if (OVRInput.GetDown(OVRInput.RawButton.A))
            {
				Pointdown();
				Skip();//文章スキップ
			}

			if (OVRInput.GetDown(OVRInput.RawButton.B))
			{
				Skip();//文章スキップ

			}

			Pointenter();


			if(textcount>4)
            {
				Rtext.text = "";
            }
		}

		private void scenecange()
        {

				SceneManager.LoadScene("Main_scene");

		}


		//==============================================================================
		/// <summary>
		/// アタッチされた時や Reset された時に呼び出されます
		/// </summary>
		private void Reset()
		{
			m_textUI = GetComponent<TMP_Text>();
		}

		/// <summary>
		/// 破棄される時に呼び出されます
		/// </summary>
		private void OnDestroy()
		{
			m_tween?.Kill();

			m_tween = null;
			m_onComplete = null;
		}

		/// <summary>
		/// 演出を再生します
		/// </summary>
		/// <param name="text">表示するテキスト ( リッチテキスト対応 )</param>
		/// <param name="speed">表示する速さ ( speed == 1 の場合 1 文字の表示に 1 秒、speed == 2 の場合 0.5 秒かかる )</param>
		/// <param name="onComplete">演出完了時に呼び出されるコールバック</param>
		public void Play( string text, float speed, Action onComplete )
		{
			m_textUI.text = text;
			m_onComplete = onComplete;

			m_textUI.ForceMeshUpdate();

			m_parsedText = m_textUI.GetParsedText();

			var length = m_parsedText.Length;
			var duration = 1 / speed * length;

			OnUpdate( 0 );

			m_tween?.Kill();
			m_tween = DOTween
				.To( value => OnUpdate( value ), 0, 1, duration )
				.SetEase( Ease.Linear )
				.OnComplete( () => OnComplete() )
			;
		}

		/// <summary>
		/// 演出をスキップします
		/// </summary>
		/// <param name="withCallbacks">演出完了時に呼び出されるコールバックを実行する場合 true</param>
		public void Skip( bool withCallbacks = true )
		{
			m_tween?.Kill();
			m_tween = null;

			OnUpdate( 1 );

			if ( !withCallbacks ) return;

			m_onComplete?.Invoke();
			m_onComplete = null;
		}

		/// <summary>
		/// 演出を一時停止します
		/// </summary>
		public void Pause()
		{
			m_tween?.Pause();
		}

		/// <summary>
		/// 演出を再開します
		/// </summary>
		public void Resume()
		{
			m_tween?.Play();
		}

		/// <summary>
		/// 演出を更新する時に呼び出されます
		/// </summary>
		private void OnUpdate( float value )
		{
			var current = Mathf.Lerp( 0, m_parsedText.Length, value );
			var count = Mathf.FloorToInt( current );

			m_textUI.maxVisibleCharacters = count;
		}

		/// <summary>
		/// 演出が更新した時に呼び出されます
		/// </summary>
		private void OnComplete()
		{
			m_tween = null;
			m_onComplete?.Invoke();
			m_onComplete = null;
		}


    }
}