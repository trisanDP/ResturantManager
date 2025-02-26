using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityAsyncAwaitUtil;


public class GameInitiator : MonoBehaviour
{
    [SerializeField]private Camera m_Camera;
    [SerializeField]private Light m_Light;
    [SerializeField]private UpdateManager m_UpdateManager;
/*    [SerializeField]private PlayerScript playerScript;*/
    [SerializeField]private GameObject Level;


    [SerializeField]private int _loadingScree;
    public void Awake() {
        
    }
/*    public async Start() {
        BindingObject();

        yield return StartCoroutine(BindingObject());
    }

    private void BindingObject() {
        *//*yield return *//*m_Camera = Instantiate(m_Camera);
        *//*m_Light = Instantiate(m_Light);*/
        /*yield return *//*Level = Instantiate(Environment);
        *//*yield return *//*m_UpdateManager = Instantiate(m_UpdateManager);
    }*/

    

}
