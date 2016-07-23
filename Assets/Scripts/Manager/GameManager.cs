using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum CurrentStage { PreTurn, Turn, PostTurn, Spectating }
public enum PlayerWeapons { Bazooka }

public class GameManager : MonoBehaviour {

    public List<Team> teams;
    public Aim reticle;
    public Image powerFill;

    public int matchLength;
    public int turnLength;
    public int preTurnLength;
    public int postTurnLength;

    [HideInInspector]
    public float matchLengthDelta;
    [HideInInspector]
    public float turnLengthDelta;
    [HideInInspector]
    public float preTurnLengthDelta;
    [HideInInspector]
    public float postTurnLengthDelta;

    bool preTurnOver;

    public CurrentStage currentStage;

    public Weapon currentWeapon;
    public WeaponWheel weaponWheel;

    public Transform playerPrefab;
    GameCamera cam;
    Dictionary<Player, int> toDamage;

    [Header("Weapons")]
    public List<Weapon> weapons;

    public delegate void Action();
    public static event Action StartTurn;

    int currentTeam;

    void Start()
    {
        toDamage = new Dictionary<Player, int>();

        Transform p1 = (Transform)Instantiate(playerPrefab, new Vector3(0f, 5f), Quaternion.identity);
        p1.GetComponent<Player>().charName = "Yellow #1";
        Transform p2 = (Transform)Instantiate(playerPrefab, new Vector3(2f, 6f), Quaternion.identity);
        p2.GetComponent<Player>().charName = "Yellow #2";
        Team t1 = new Team();
        t1.teamColour = TeamColour.Yellow;
        t1.SetPlayers(new List<Player>() { p1.GetComponent<Player>(), p2.GetComponent<Player>() });

        Transform p3 = (Transform)Instantiate(playerPrefab, new Vector3(1f, 5f), Quaternion.identity);
        p3.GetComponent<Player>().charName = "Green #1";
        Transform p4 = (Transform)Instantiate(playerPrefab, new Vector3(3f, 5f), Quaternion.identity);
        p4.GetComponent<Player>().charName = "Green #2";
        Team t2 = new Team();
        t2.teamColour = TeamColour.Green;
        t2.SetPlayers(new List<Player>() { p3.GetComponent<Player>(), p4.GetComponent<Player>() });

        teams = new List<Team>() { t1, t2 };

        cam = GameObject.Find("Main Camera").GetComponent<GameCamera>();
        cam.TargetDestroyed += TrackerDestroyed;

        GetComponent<OverlayManager>().enabled = true;

        StartGame(teams);
    }

    void Update()
    {
        currentWeapon = weaponWheel.SelectedWeapon;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnEnd();
        }
        if (currentStage == CurrentStage.Turn)
        {
            matchLengthDelta -= Time.deltaTime;
            turnLengthDelta -= Time.deltaTime;
        }
        if (currentStage == CurrentStage.PreTurn)
            preTurnLengthDelta -= Time.deltaTime;
        if (currentStage == CurrentStage.PostTurn)
            postTurnLengthDelta -= Time.deltaTime;

        if (preTurnLengthDelta <= 0)
            PreTurnEnd();

        if (turnLengthDelta <= 0)
            TurnEnd();

        if (postTurnLengthDelta <= 0)
            PostTurnEnd();

        powerFill.fillAmount = CurrentPlayer.power;
    }

    public void RegisterDamage(Player player, int damage)
    {
        if (toDamage.ContainsKey(player))
            toDamage[player] += damage;
        else
            toDamage.Add(player, damage);
    }

    void TrackerDestroyed()
    {
        if (toDamage.Count != 0)
            StartCoroutine(DealDamage());
        else
            currentStage = CurrentStage.PostTurn;
    }

    public void GiveExplosive(Explosive explosive)
    {
        GetComponent<OverlayManager>().GiveExplosive(explosive);
    }

    IEnumerator DealDamage()
    {
        currentStage = CurrentStage.Spectating;
        foreach (Player player in toDamage.Keys)
        {
            while (player.GetComponent<Rigidbody2D>().velocity != Vector2.zero) { yield return new WaitForFixedUpdate(); }
        }
        foreach (Player player in toDamage.Keys)
        {
            player.attacked = false;
            cam.SetTarget(player.transform);
            yield return new WaitForSeconds(0.25f);
            for (int i = 0; i < toDamage[player]; i++)
            {
                player.health--;
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(0.25f);
        }
        toDamage = new Dictionary<Player, int>();
        cam.SetTarget(CurrentPlayer.transform);
        PostTurnEnd();
    }

    public void StartGame(List<Team> teams)
    {
        this.teams = teams;
        currentTeam = 0;
        matchLengthDelta = matchLength;
        preTurnLengthDelta = preTurnLength;
        turnLengthDelta = turnLength;
        postTurnLengthDelta = postTurnLength;
        TakeTurn();
    }

    void PreTurnEnd()
    {
        if (preTurnOver)
            return;
        currentStage = CurrentStage.Turn;
        preTurnLengthDelta = preTurnLength;
        preTurnOver = true;
    }

    void TurnEnd()
    {
        turnLengthDelta = turnLength;
        CurrentPlayer.isDangerous = false;
        if (CurrentPlayer.fired != null)
        {
            cam.SetTarget(CurrentPlayer.fired.transform);
            currentStage = CurrentStage.Spectating;
            cam.waitForDestroy = true;
        }
        else
            currentStage = CurrentStage.PostTurn;
    }

    void PostTurnEnd()
    {
        postTurnLengthDelta = postTurnLength;
        toDamage = new Dictionary<Player, int>();
        AdvanceTurn();
        TakeTurn();
    }

    public Player CurrentPlayer
    {
        get
        {
            Team cTeam = teams[currentTeam];
            return cTeam.players[cTeam.currentPlayer];
        }
        set
        {
            Team cTeam = teams[currentTeam];
            cTeam.players[cTeam.currentPlayer] = value;
        }
    }

    public Team CurrentTeam
    {
        get { return teams[currentTeam]; }
        set { teams[currentTeam] = value; }
    }

    public List<Player> Players
    {
        get
        {
            List<Player> players = new List<Player>();
            foreach (Team team in teams)
                foreach (Player player in team.players)
                    players.Add(player);
            return players;
        }
    }

    void NextTeam()
    {
        currentTeam++;
        if (currentTeam == teams.Count)
            currentTeam = 0;
    }

    public void TakeTurn()
    {
        currentStage = CurrentStage.PreTurn;
        preTurnOver = false;
        CurrentPlayer.isTurn = true;
        CurrentPlayer.isDangerous = true;

        Camera.main.transform.GetComponent<GameCamera>().SetTarget(CurrentPlayer.transform);
        CurrentPlayer.PreTurnEnd += PreTurnEnd;
        CurrentPlayer.TurnEnd += TurnEnd;
        if(reticle != null)
            reticle.root = CurrentPlayer.transform;
    }

    void AdvanceTurn()
    {
        CurrentPlayer.isTurn = false;
        CurrentPlayer.PreTurnEnd -= PreTurnEnd;
        CurrentPlayer.TurnEnd -= TurnEnd;
        NextTeam();
        CurrentTeam.NextPlayer();
    }
}
