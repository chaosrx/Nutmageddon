using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OverlayManager : MonoBehaviour {

    GameManager gameManager;
    Dictionary<TeamColour, Color> colour;
    Dictionary<TeamColour, Sprite> colourBar;
    Dictionary<TeamColour, Sprite> colourReticle;
    Dictionary<TeamColour, Sprite> colourArrow;
    Dictionary<TeamColour, Sprite> colourHeadsLeft;
    Dictionary<TeamColour, Sprite> colourHeadsRight;
    Dictionary<TeamColour, Sprite> colourTeamBar;
    Dictionary<TeamColour, Sprite> colourOverlay;

    // Tag Manager
    [Header("Tag Manager")]
    public RectTransform parent;
    public RectTransform tagPrefab;
    public Sprite[] colourBars;
    public Sprite[] numbers;
    GameObject[] players;
    GameObject[] tags;
    Explosive currentExplosive;

    // General Manager
    [Header("General Manager")]
    public Sprite[] reticleSprites;
    public Sprite[] arrowSprites;
    public RuntimeAnimatorController bobAnimator;
    public RectTransform explosiveCounterBox;
    public Image explosiveCounter;
    GameObject reticle;
    GameObject weaponArms;
    GameObject arrow;
    GameObject arrowAnimation;
    GameObject generalParent;

    [Header("Team health")]
    public Sprite[] heads;
    public Sprite[] barBack;
    public Sprite[] barOverlay;
    
    public Image headLeft;
    public Image headRight;
    public Image barLeft;
    public Image barRight;
    public Image overlayLeft;
    public Image overlayRight;
    public Slider teamHealthLeft;
    public Slider teamHealthRight;

    void Start()
    {
        gameManager = GetComponent<GameManager>();

        // Set up colours.
        colour = new Dictionary<TeamColour, Color>();
        colour.Add(TeamColour.Blue, new Color(0f / 255f, 216f / 255f, 255f / 255f));
        colour.Add(TeamColour.Green, new Color(113f / 255f, 209f / 255f, 44f / 255f));
        colour.Add(TeamColour.Red, new Color(218f / 255f, 82f / 255f, 64f / 255f));
        colour.Add(TeamColour.Yellow, new Color(252f / 255f, 233f / 255f, 56f / 255f));

        colourBar = new Dictionary<TeamColour, Sprite>();
        colourBar.Add(TeamColour.Blue, colourBars[0]);
        colourBar.Add(TeamColour.Green, colourBars[1]);
        colourBar.Add(TeamColour.Red, colourBars[2]);
        colourBar.Add(TeamColour.Yellow, colourBars[3]);

        colourReticle = new Dictionary<TeamColour, Sprite>();

        // Tag Manager
        players = GameObject.FindGameObjectsWithTag("Player");
        tags = new GameObject[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            GameObject tag = Instantiate(tagPrefab.gameObject);
            PlayerTag pt = tag.GetComponent<PlayerTag>();
            pt.spaceText.text = players[i].GetComponent<Player>().charName;         // This isn't visible, but sets the correct width.
            pt.visibleText.text = players[i].GetComponent<Player>().charName;
            TeamColour tc = players[i].GetComponent<Player>().team.teamColour;
            pt.visibleText.color = colour[tc];
            pt.bar.sprite = colourBar[tc];
            tag.transform.SetParent(transform, false);
            tag.transform.parent = parent;
            tags[i] = tag;
        }

        // General Manager
        colourReticle = new Dictionary<TeamColour, Sprite>();
        colourReticle.Add(TeamColour.Blue, reticleSprites[0]);
        colourReticle.Add(TeamColour.Green, reticleSprites[1]);
        colourReticle.Add(TeamColour.Red, reticleSprites[2]);
        colourReticle.Add(TeamColour.Yellow, reticleSprites[3]);

        colourArrow = new Dictionary<TeamColour, Sprite>();
        colourArrow.Add(TeamColour.Blue, arrowSprites[0]);
        colourArrow.Add(TeamColour.Green, arrowSprites[1]);
        colourArrow.Add(TeamColour.Red, arrowSprites[2]);
        colourArrow.Add(TeamColour.Yellow, arrowSprites[3]);

        generalParent = new GameObject("Overlay");

        reticle = new GameObject("Reticle");
        reticle.AddComponent<SpriteRenderer>().sortingOrder = 1000;
        reticle.transform.parent = generalParent.transform;

        weaponArms = new GameObject("WeaponArms");

        if (gameManager.CurrentPlayer.currentWeapon.arms != null)
            weaponArms.AddComponent<SpriteRenderer>().sprite = gameManager.CurrentPlayer.currentWeapon.arms;

        weaponArms.GetComponent<SpriteRenderer>().sortingOrder = 10001;
        weaponArms.transform.parent = generalParent.transform;

        arrow = new GameObject("Arrow");
        arrowAnimation = new GameObject("ArrowAnimation");
        arrowAnimation.AddComponent<SpriteRenderer>().sortingOrder = 1000;
        arrowAnimation.AddComponent<Animator>().runtimeAnimatorController = bobAnimator;
        arrowAnimation.GetComponent<SpriteRenderer>().sprite = arrowSprites[0];
        arrowAnimation.transform.parent = arrow.transform;
        arrow.transform.parent = generalParent.transform;

        colourHeadsLeft = new Dictionary<TeamColour, Sprite>();
        colourHeadsLeft.Add(TeamColour.Blue, heads[0]);
        colourHeadsLeft.Add(TeamColour.Green, heads[1]);
        colourHeadsLeft.Add(TeamColour.Red, heads[2]);
        colourHeadsLeft.Add(TeamColour.Yellow, heads[3]);

        colourHeadsRight = new Dictionary<TeamColour, Sprite>();
        colourHeadsRight.Add(TeamColour.Blue, heads[4]);
        colourHeadsRight.Add(TeamColour.Green, heads[5]);
        colourHeadsRight.Add(TeamColour.Red, heads[6]);
        colourHeadsRight.Add(TeamColour.Yellow, heads[7]);

        colourTeamBar = new Dictionary<TeamColour, Sprite>();
        colourTeamBar.Add(TeamColour.Blue, colourBars[0]);
        colourTeamBar.Add(TeamColour.Green, colourBars[1]);
        colourTeamBar.Add(TeamColour.Red, colourBars[2]);
        colourTeamBar.Add(TeamColour.Yellow, colourBars[3]);

        colourOverlay = new Dictionary<TeamColour, Sprite>();
        colourOverlay.Add(TeamColour.Blue, barOverlay[0]);
        colourOverlay.Add(TeamColour.Green, barOverlay[1]);
        colourOverlay.Add(TeamColour.Red, barOverlay[2]);
        colourOverlay.Add(TeamColour.Yellow, barOverlay[3]);

        headLeft.sprite = colourHeadsLeft[gameManager.teams[0].teamColour];
        headRight.sprite = colourHeadsRight[gameManager.teams[1].teamColour];
        overlayLeft.sprite = colourOverlay[gameManager.teams[0].teamColour];
        overlayRight.sprite = colourOverlay[gameManager.teams[1].teamColour];
        barLeft.sprite = colourTeamBar[gameManager.teams[0].teamColour];
        barRight.sprite = colourTeamBar[gameManager.teams[1].teamColour];
        teamHealthLeft.maxValue = gameManager.teams[0].players.Count * 100f;
        teamHealthRight.maxValue = gameManager.teams[1].players.Count * 100f;
    }

    void Update()
    {
        // Tag Manager
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<Player>().health > 0f)
            {
                // Place tags above players' heads.
                Vector2 screenPoint = Camera.main.WorldToScreenPoint(players[i].transform.position + new Vector3(0.0f, 0.85f));
                tags[i].GetComponent<RectTransform>().position = screenPoint;
                // Only show names when it's a turn isn't in progress.
                tags[i].transform.GetChild(0).gameObject.SetActive(gameManager.currentStage != CurrentStage.Turn);
                // Show everyone's health if a turn is in progress.
                tags[i].transform.GetChild(1).gameObject.SetActive(players[i] != gameManager.CurrentPlayer.gameObject || gameManager.currentStage != CurrentStage.Turn);
                tags[i].GetComponent<PlayerTag>().health.value = players[i].GetComponent<Player>().health;
                if (gameManager.CurrentPlayer == players[i])
                    tags[i].transform.SetAsLastSibling();
            }
            else
            {
                tags[i].transform.GetChild(0).gameObject.SetActive(false);
                tags[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        // Reticle Manager
        float angle = gameManager.CurrentPlayer.angle;
        reticle.transform.position = gameManager.CurrentPlayer.transform.position + Quaternion.AngleAxis(angle, Vector3.forward) * new Vector3(1, 0);
        reticle.GetComponent<SpriteRenderer>().sprite = colourReticle[gameManager.CurrentTeam.teamColour];
        reticle.SetActive(gameManager.currentStage == CurrentStage.Turn);
        float scale = gameManager.CurrentPlayer.transform.localScale.x;

        weaponArms.transform.position = gameManager.CurrentPlayer.transform.position + new Vector3(0.1f * scale, -0.05f);
        if (gameManager.CurrentPlayer.currentWeapon.weaponType == WeaponType.Normal)
        {
            Sprite arms;
            Weapon nw = gameManager.CurrentPlayer.currentWeapon;
            if (gameManager.currentStage == CurrentStage.Turn)
                arms = nw.arms;
            else
                if (nw.armsAfter == null)
                    arms = nw.arms;
                else
                    arms = nw.armsAfter;
            weaponArms.transform.rotation = Quaternion.AngleAxis(angle - gameManager.CurrentPlayer.currentWeapon.offset * scale, Vector3.forward);
            weaponArms.GetComponent<SpriteRenderer>().sprite = arms;
        }
        weaponArms.GetComponent<SpriteRenderer>().flipY = scale == -1;
        weaponArms.gameObject.SetActive(!gameManager.CurrentPlayer.GetComponent<Animator>().GetBool("walking") && (gameManager.currentStage != CurrentStage.PostTurn && gameManager.currentStage != CurrentStage.Spectating));

        arrow.SetActive(gameManager.currentStage == CurrentStage.PreTurn);
        arrowAnimation.GetComponent<SpriteRenderer>().sprite = colourArrow[gameManager.CurrentTeam.teamColour];
        arrow.transform.position = gameManager.CurrentPlayer.transform.position + new Vector3(0f, 1.5f);

        float lHealth = 0f;
        foreach (Player player in gameManager.teams[0].players)
            lHealth += player.health;
        teamHealthLeft.value = lHealth;

        float rHealth = 0f;
        foreach (Player player in gameManager.teams[1].players)
            rHealth += player.health;
        teamHealthRight.value = rHealth;

        if (currentExplosive != null)
        {
            if (currentExplosive.timer > 0)
            {
                explosiveCounterBox.gameObject.SetActive(true);
                explosiveCounter.gameObject.SetActive(true);
                explosiveCounterBox.position = Camera.main.WorldToScreenPoint(currentExplosive.transform.position + new Vector3(-0.5f, 0.5f));
                explosiveCounter.sprite = numbers[Mathf.CeilToInt(currentExplosive.timeDelta)];
            }
            else
            {
                explosiveCounterBox.gameObject.SetActive(false);
                explosiveCounter.gameObject.SetActive(false);
            }
        }
        else
        {
            explosiveCounterBox.gameObject.SetActive(false);
            explosiveCounter.gameObject.SetActive(false);
        }
    }

    public void GiveExplosive(Explosive explosive)
    {
        currentExplosive = explosive;
    }
}
