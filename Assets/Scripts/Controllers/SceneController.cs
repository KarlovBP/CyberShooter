using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    private static SceneController Instance;

    private string CurrentSceneName;
    private string NextSceneName;
    private const string InitialSceneName = "Main Menu Scene";
    private AsyncOperation ResourceUnloadTask;
    private AsyncOperation SceneLoadTask;
    private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count }
    private SceneState CurrentSceneState;
    private delegate void UpdateDelegate();
    private UpdateDelegate[] UpdateDelegates;
    
    public static void SwitchScene (string nextSceneName)
    {
        //if (Controller != null)
        //{
            if (Instance.CurrentSceneName != nextSceneName)
            {
                Instance.NextSceneName = nextSceneName;
            }
        //}
    }

    protected void Awake ()
    {
        //Setup singleton instance
        Instance = this;

        //Keep this object alive between scene changes
        DontDestroyOnLoad(gameObject);

        //Setup the array of UpdateDelegates
        UpdateDelegates = new UpdateDelegate[(int)SceneState.Count];

        //Set each UpdateDelegate
        UpdateDelegates[(int)SceneState.Reset] = UpdateSceneReset;
        UpdateDelegates[(int)SceneState.Preload] = UpdateScenePreload;
        UpdateDelegates[(int)SceneState.Load] = UpdateSceneLoad;
        UpdateDelegates[(int)SceneState.Unload] = UpdateSceneUnload;
        UpdateDelegates[(int)SceneState.Postload] = UpdateScenePostload;
        UpdateDelegates[(int)SceneState.Ready] = UpdateSceneReady;
        UpdateDelegates[(int)SceneState.Run] = UpdateSceneRun;

        NextSceneName = InitialSceneName;
        CurrentSceneState = SceneState.Reset;
        //camera.ortographicSize = Screen.height / 2;
    }

    protected void OnDestroy ()
    {
        //Clean up all the UpdateDelegates
        if (UpdateDelegates != null)
        {
            for (int i = 0; i < (int)SceneState.Count; i++)
            {
                UpdateDelegates[i] = null;
            }
            UpdateDelegates = null;
        }

        //Clean up the singleton instance
        if (Instance != null)
        {
            Instance = null;
        }
    }

    protected void OnDisable ()
    {

    }

    protected void OnEnable()
    {

    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    if (UpdateDelegates[(int)CurrentSceneState] != null)
        {
            UpdateDelegates[(int)CurrentSceneState]();
        }
	}

    //Attach the new scene controller to start cascade of loading
    private void UpdateSceneReset ()
    {
        //Run a Garbage Collector pass
        System.GC.Collect();
        CurrentSceneState = SceneState.Preload;
    }

    //Handle anything that needs to happen before oading
    private void UpdateScenePreload()
    {
        SceneLoadTask = SceneManager.LoadSceneAsync(NextSceneName);
        CurrentSceneState = SceneState.Load;
    }

    //Show the loading screen until it's loaded
    private void UpdateSceneLoad()
    {
        //Done loading?
        if (SceneLoadTask.isDone)
        {
            CurrentSceneState = SceneState.Unload;
        }
        else
        {
            //Update scene loading progress
        }
    }

    //Clean up unused resources bu unloading them
    private void UpdateSceneUnload()
    {
        //Did not start cleaning up resources yet?
        if (ResourceUnloadTask == null)
        {
            ResourceUnloadTask = Resources.UnloadUnusedAssets();
        }
        //Done cleaning up?
        else if (ResourceUnloadTask.isDone)
        {
            ResourceUnloadTask = null;
            CurrentSceneState = SceneState.Postload;
        }
    }

    //Handle anything that needs to happen immediately after loading
    private void UpdateScenePostload()
    {
        CurrentSceneName = NextSceneName;
        CurrentSceneState = SceneState.Ready;
    }

    //Handle anything that needs to happen immediately before running
    private void UpdateSceneReady()
    {
        //Run a Garbage Collector pass
        //If we have assets loaded in the scene that are
        //currently unused but may be used later
        //DON'T do this here
        System.GC.Collect();
        CurrentSceneState = SceneState.Run;
    }

    //Wait for scene change
    private void UpdateSceneRun()
    {
        if (CurrentSceneName != NextSceneName)
        {
            CurrentSceneState = SceneState.Reset;
        }
    }
}
