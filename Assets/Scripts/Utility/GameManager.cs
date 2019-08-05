using System.Collections;
using System.Linq;
using ATT.Interaction.PuzzleSystem;
using ATT.Interaction.ReactionSystem;
using ATT.SpecialEffects;
using ATT.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR.InteractionSystem;

namespace ATT
{
    public class GameManager : MonoBehaviour
    {
        private const string chewToyPuzzle = "Chew Toy";
        private Vector3 defaultGravity = new Vector3(0, -9.81f, 0);

        [Header("Time")]
        [HideInInspector] public Timer Timer = null;
        [SerializeField] private float countdownTimerSeconds = 120f;
        [SerializeField] private float endGameWaitSeconds = 5f;

        [Header("Puzzles")]
        public PuzzleManager PuzzleManager = null;
        public ChewToy ChewToy = null;
        public TeaCupTrigger[] TeaCupTriggers = null;
        public PosterCollider PosterTrigger = null;

        [Header("Teleportation")]
        public TeleportArea EndGameTeleport = null;
        public Teleport Teleport = null;

        [Header("Player")]
        public Player Player = null;
        public Grow GrowPlayer = null;
        public PlayerVitals PlayerVitals = null;

        [Header("End Game")]
        [SerializeField] private bool playerWon = false;
        public LoseSequenceManager LoseSequence = null;
        public WinSequenceManager WinSequence = null;

        [Header("Misc")]
        public BunnyClock BunnyClock = null;
        
        private WaitForSecondsRealtime endGameWait = null;

        #region Unity Methods
        void Awake()
        {
            Physics.gravity = defaultGravity;
        }

        void Start()
        {
            SetAndCheckReferences();

            SubscribeToPuzzleRelatedEvents();
            SubscribeToTimerEvents();
                      
            SetTeleportMarkersActiveState(false);  
        }
        #endregion

        /// <summary>
        /// Locks / unlocks <see cref="TeleportMarkerBase"/> array in the <see cref="Teleport"/> script.
        /// </summary>
        /// <param name="active">Whether the <see cref="TeleportMarkerBase"/> status should be locked.</param>
        public void SetTeleportMarkersActiveState(bool active)
        {
            if (Teleport.instance != null)
            {
                Teleport.instance.TeleportMarkers
                    .ToList()
                    .ForEach(t =>
                    {
                        t.markerActive = active;
                    });
            }
        }

        private void TriggerPlayerGrowBehaviour()
        {
            GrowPlayer.StartGrowingPlayer();
        }

        public void GrowComplete()
        {
            SetTeleportMarkersActiveState(true);
            EndGameTeleport.markerActive = false;
            StartCountdownTimer();
        }

        private void StartCountdownTimer()
        {
            Timer.StartCountdownTimer(countdownTimerSeconds);
        }
        
        private void StartBunnyClockBehaviour()
        {
            if (BunnyClock != null)
            {
                BunnyClock.TriggerClockHandRotation();
            }
        }

        private void SetCountdownReachedCriticalBehaviour()
        {
            PlayerVitals.StartHeartbeat();            
        }

        public void StopPlayerVitalsAudio()
        {
            PlayerVitals.Loop = false;
        }

        #region End Game
        private void TriggerEndGame()
        {
            DisablePuzzleSolve();

            Timer.StopCountdownTimer();
            PlayerVitals.StopHeartbeat();
            BunnyClock.StopClockHandRotation();

            if (PuzzleManager.AllPuzzlesSolved)
            {
                StartCoroutine(WaitAndTriggerWinSequence());                
            }
            else
            {
                LoseSequence.TriggerLoseSequence();
            }

            SetTeleportMarkersActiveState(false);
            EndGameTeleport.gameObject.SetActive(true);
        }

        private void DisablePuzzleSolve()
        {
            DisablePuzzleInteraction();

            if (PosterTrigger != null)
            {
                PosterTrigger.enabled = false;
            }

            if (TeaCupTriggers != null && TeaCupTriggers.Length > 0)
            {
                TeaCupTriggers.ForEach(t => t.enabled = false);
            }
        }

        public void DisablePuzzleInteraction()
        {
            FindObjectsOfType<ReactionCollection>().ForEach(r =>
            {
                r.DisableReactionCollectionReactions();
            });
        }

        IEnumerator WaitAndTriggerWinSequence()
        {
            yield return endGameWait;
            WinSequence.TriggerWinSequence();
            yield break;
        }
        #endregion

        #region Event Subscriptions
        private void SubscribeToPuzzleRelatedEvents()
        {
            if(PuzzleManager == null)
            {
                PuzzleManager = FindObjectOfType<PuzzleManager>();
            }
            Assert.IsNotNull(PuzzleManager, $"<b>[GameManager]</b> Puzzle Manager is not been assigned and cannot be found in the scene.");
            PuzzleManager.OnAllPuzzlesCompleted += TriggerEndGame;

            if (ChewToy == null)
            {
                ChewToy = FindObjectOfType<ChewToy>();
            }
            Assert.IsNotNull(ChewToy, $"<b>[GameManager]</b> Chew Toy is not been assigned and cannot be found in the scene.");
            ChewToy.OnChewToyChewed += TriggerPlayerGrowBehaviour;

            if(TeaCupTriggers == null || TeaCupTriggers.Length == 0)
            {
                TeaCupTriggers = FindObjectsOfType<TeaCupTrigger>();
            }
            Assert.IsNotNull(TeaCupTriggers, $"<b>[GameManager]</b> TeaCupTriggers are not assigned and cannot be found in the scene.");
            Assert.IsTrue(TeaCupTriggers.Length == 4, $"<b>[GameManager]</b> TeaCupTriggers array length is {TeaCupTriggers.Length}. Should be 4.");

            if (PosterTrigger == null)
            {
                PosterTrigger = FindObjectOfType<PosterCollider>();
            }
            Assert.IsNotNull(TeaCupTriggers, $"<b>[GameManager]</b> Poster Collider is not assigned and cannot be found in the scene.");
        }

        private void SubscribeToTimerEvents()
        {
            if (Timer == null)
            {
                Timer = GetComponent<Timer>();
            }
            Assert.IsNotNull(Timer, $"<b>[GameManager]</b> Timer is not been assigned and cannot be found in the scene.");

            Timer.OnCountdownReachedWarning += StartBunnyClockBehaviour;
            Timer.OnCountdownReachedCritical += SetCountdownReachedCriticalBehaviour;
            Timer.OnCountdownEnded += TriggerEndGame;            
        }        
        #endregion

        #region Debugging
        private void SetAndCheckReferences()
        {
            if(Player == null)
            {
                Player = Player.instance;
            }
            Assert.IsNotNull(Player, $"<b>[GameManager]</b> Player is not assigned and cannot be found in the scene.");

            if (GrowPlayer == null)
            {
                Player.GetComponent<Grow>();
            }
            Assert.IsNotNull(GrowPlayer, $"<b>[GameManager]</b> GrowPlayer is not been assigned and cannot be found on the Player game object.");

            if (PlayerVitals == null)
            {
                PlayerVitals = Player.GetComponentInChildren<PlayerVitals>();
            }
            Assert.IsNotNull(PlayerVitals, $"<b>[GameManager]</b> PlayerVitals script is not assigned and cannot be found in the Player game object's children.");

            if(Teleport == null)
            {
                Teleport = Teleport.instance;
            }
            Assert.IsNotNull(Teleport, $"<b>[GameManager]</b> Teleport is not assigned and cannot be found in the scene.");

            if(BunnyClock == null)
            {
                BunnyClock = FindObjectOfType<BunnyClock>();
            }
            Assert.IsNotNull(BunnyClock, $"<b>[GameManager]</b> BunnyClock is not assigned and cannot be found in the scene.");

            if(WinSequence == null)
            {
                WinSequence = FindObjectOfType<WinSequenceManager>();
            }
            Assert.IsNotNull(WinSequence, $"<b>[GameManager]</b> WinSequence is not assigned and cannot be found in the scene.");

            if(LoseSequence == null)
            {
                LoseSequence = FindObjectOfType<LoseSequenceManager>();
            }
            Assert.IsNotNull(LoseSequence, $"<b>[GameManager]</b> LoseSequence is not assigned and cannot be found in the scene.");
            
            SubscribeToPuzzleRelatedEvents();
            SubscribeToTimerEvents();
        }
        #endregion
    }
}