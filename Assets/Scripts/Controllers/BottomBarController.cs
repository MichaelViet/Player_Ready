using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BottomBarController : MonoBehaviour
{

    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;

    private int sentenceIndex = -1;
    private StoryScene currentScene;
    private State state = State.COMPLETED;
    private Animator animator;
    private bool isHidden = false;

    public Dictionary<Speaker, SpriteController> sprites;
    public GameObject spritesPrefab;

    private Coroutine typingCoroutine;
    private float speedFactor = 1f;

    private enum State
    {
        PLAYING, SPEEDED_UP, COMPLETED
    }

    private void Start()
    {

        sprites = new Dictionary<Speaker, SpriteController>();
        animator = GetComponent<Animator>();
    }

    public int GetSentenceIndex()
    {
        return sentenceIndex;
    }

    public void Hide()
    {
        if (!isHidden)
        {
            animator.SetTrigger("Hide");
            isHidden = true;
        }
    }

    public void Show()
    {
        animator.SetTrigger("Show");
        isHidden = false;
    }

    public void ClearText()
    {
        barText.text = "";
        personNameText.text = "";
    }

    public void PlayScene(StoryScene scene, int sentenceIndex = -1, bool isAnimated = true)
    {
        currentScene = scene;
        this.sentenceIndex = sentenceIndex;
        PlayNextSentence(isAnimated);
    }

    public void PlayNextSentence(bool isAnimated = true)
    {
        sentenceIndex++;
        PlaySentence(isAnimated);
    }

    public void GoBack()
    {
        sentenceIndex--;
        StopTyping();
        ClearText();
        HideSprites();
        PlaySentence(false);
    }

    public bool IsCompleted()
    {
        return state == State.COMPLETED || state == State.SPEEDED_UP;
    }

    public bool IsLastSentence()
    {
        return sentenceIndex + 1 == currentScene.sentences.Count;
    }

    public bool IsFirstSentence()
    {
        return sentenceIndex == 0;
    }

    public void SpeedUp()
    {
        state = State.SPEEDED_UP;
        speedFactor = 0.25f;
    }

    public void StopTyping()
    {
        state = State.COMPLETED;
        StopCoroutine(typingCoroutine);
    }

    public void HideSprites()
    {
        while (spritesPrefab.transform.childCount > 0)
        {
            DestroyImmediate(spritesPrefab.transform.GetChild(0).gameObject);
        }
        sprites.Clear();
    }

    private void PlaySentence(bool playAnimation = true)
    {
        speedFactor = 1f;
        var currentSentence = currentScene.sentences[sentenceIndex];
        typingCoroutine = StartCoroutine(TypeText(currentSentence.text));
        personNameText.text = currentSentence.speaker.speakerName;
        personNameText.color = currentSentence.speaker.textColor;
        ActSpeakers(playAnimation);
    }

    private IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.PLAYING;

        for (int i = 0; i < text.Length; i++)
        {
            barText.text += text[i];
            yield return new WaitForSeconds(speedFactor * 0.05f);

            if (state == State.COMPLETED)
            {
                break;
            }
        }

        state = State.COMPLETED;
    }

    private void ActSpeakers(bool playAnimation = true)
    {
        var actions = currentScene.sentences[sentenceIndex].actions;
        foreach (var action in actions)
        {
            ActSpeaker(action, playAnimation);
        }
    }

    private void ActSpeaker(StoryScene.Sentence.Action action, bool playAnimation = true)
    {
        SpriteController controller;
        if (!sprites.TryGetValue(action.speaker, out controller))
        {
            GameObject prefab = action.speaker.prefab.gameObject;
            GameObject instance = Instantiate(prefab, spritesPrefab.transform);
            controller = instance.GetComponent<SpriteController>();
            sprites.Add(action.speaker, controller);
        }

        switch (action.actionType)
        {
            case StoryScene.Sentence.Action.Type.APPEAR:
                controller.Setup(action.speaker.sprites[action.spriteIndex]);
                controller.Show(action.coords, playAnimation);
                break;
            case StoryScene.Sentence.Action.Type.MOVE:
                controller.Move(action.coords, action.moveSpeed, playAnimation);
                break;
            case StoryScene.Sentence.Action.Type.DISAPPEAR:
                controller.Hide(playAnimation);
                break;
        }
        controller.SwitchSprite(action.speaker.sprites[action.spriteIndex], playAnimation);
    }
    public void Update()
    {

    }
}
