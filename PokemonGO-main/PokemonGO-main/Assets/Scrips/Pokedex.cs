using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class Pokedex : MonoBehaviour
{
    public GameObject pokedex;
    public GameObject btnPokedex;

    [Header("Url Pokedex")]
    private string urlPokedex = "https://pokeapi.co/api/v2/pokemon?limit=10&offset=";
    private string urlComplete;
    private string offset;
    private int index;

    [Header("Url Images Pokedex")]
    private string urlImagesPokemons = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/"; // + 1.png
    private string urlCompleteImagesPokemons;
    private Texture2D texturePokemons;
    private List<Texture2D> imgPokemons;

    [Header("Data - Logic")]
    private DataPokemons responsePokemonsGeneral;
    public RectTransform ParentPanel;
    private GameObject[] goButtonPokemons;
    private int cantDestroy;
    

    [Header("Panel Data Pokemons")]
    public GameObject panelDataPokemons;
    private string specificPokemon;
    private Texture2D textureSpecificPokemon;
    private DataSpecificPokemon dataSpecificPokemon;
    public RectTransform ParentPanelInfoSpecific;
    private Text txtId, txtName, txtHeight, txtWeidth, txtExperience, txtLocation, txtability1, txtability2;
    private string locationPokemon;
    private string idPokemon;
    public RawImage imgSpecific;

    [Header("Buttons")]
    public Button buttonRight;
    public Button buttonLeft;
    public Button backList;
    public Button backSpecific;

    private void Start()
    {
        index = 0;
        offset = index.ToString();
       // StartCoroutine(GetPokemons());
        //  StartCoroutine(GetDataSpecificPokemon());
       // StartCoroutine(GetLocationPokemon());
    }

 

    public IEnumerator GetPokemons()
    {
        urlComplete = urlPokedex + offset;
        UnityWebRequest www = UnityWebRequest.Get(urlComplete);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Algo salio mal");
        }
        else
        {
            if (www.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
              //  Debug.Log("Json Result pokemon: " + jsonResult);

                responsePokemonsGeneral = JsonUtility.FromJson<DataPokemons>(jsonResult);
              //  Debug.Log("root del pokemon: " + responsePokemonsGeneral.results.Count);

                goButtonPokemons = new GameObject[responsePokemonsGeneral.results.Count];

                for (int i = 0; i < responsePokemonsGeneral.results.Count; i++)
                {
                    cantDestroy += 1;
                    goButtonPokemons[i] = Instantiate(Resources.Load("ButtonPokems", typeof(GameObject))) as GameObject;
                    goButtonPokemons[i].GetComponentInChildren<Text>().text = responsePokemonsGeneral.results[i].name;
                    goButtonPokemons[i].transform.SetParent(ParentPanel, false);

                    string urlPokemon = responsePokemonsGeneral.results[i].url;
                    goButtonPokemons[i].GetComponent<Button>().onClick.AddListener(() => ButtonClicked(urlPokemon));
                    
                }

                StartCoroutine(GetImagesPokemons());
            }
        }
    }

    public IEnumerator GetOneImgPokemon()
    {
            urlCompleteImagesPokemons = urlImagesPokemons + idPokemon + ".png";
          //  Debug.Log("url image: " + urlCompleteImagesPokemons);
            UnityWebRequest www = UnityWebRequest.Get(urlCompleteImagesPokemons);
            DownloadHandlerTexture textPokemon = new DownloadHandlerTexture(true);
            www.downloadHandler = textPokemon;
            www.timeout = 10;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Algo salio mal");
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                //  Debug.Log("Json Result images: " + jsonResult);

                    textureSpecificPokemon = textPokemon.texture;
                    imgSpecific.texture = textureSpecificPokemon;
                    imgSpecific.GetComponent<RawImage>().enabled = true;
                }
            }
        
        
    }

    public IEnumerator GetImagesPokemons()
    {
        for (int i = index; i < goButtonPokemons.Length + index; i++)
        {
            int aux = i + 1;
            urlCompleteImagesPokemons = urlImagesPokemons + aux + ".png";
            //  Debug.Log("url image: " + urlCompleteImagesPokemons);
            UnityWebRequest www = UnityWebRequest.Get(urlCompleteImagesPokemons);
            DownloadHandlerTexture textPokemon = new DownloadHandlerTexture(true);
            www.downloadHandler = textPokemon;
            www.timeout = 10;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Algo salio mal");
            }
            else
            {
                if (www.isDone)
                {
                    string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    //  Debug.Log("Json Result images: " + jsonResult);

                    texturePokemons = textPokemon.texture;
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().texture = texturePokemons;
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().SetNativeSize();
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().transform.localScale = new Vector2(0.5f, 0.5f);
                    goButtonPokemons[i - index].GetComponentInChildren<RawImage>().enabled = true;
                }
            }
        }

    }

    public IEnumerator GetDataSpecificPokemon()
    {
        //
       // UnityWebRequest www = UnityWebRequest.Get("https://pokeapi.co/api/v2/pokemon/1/");
        UnityWebRequest www = UnityWebRequest.Get(specificPokemon);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Algo salio mal");
        }
        else
        {
            if (www.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("Json Result pokemon: " + jsonResult);

                dataSpecificPokemon = JsonUtility.FromJson<DataSpecificPokemon>(jsonResult);
                locationPokemon = dataSpecificPokemon.location_area_encounters;

                txtId = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtId.text = "# " + dataSpecificPokemon.id.ToString();
                txtId.transform.SetParent(ParentPanelInfoSpecific, false);
                txtId.transform.localPosition = new Vector2(96, 190);
                txtId.rectTransform.sizeDelta = new Vector2(47, 30);
                

                txtName = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtName.text = "Name: " + dataSpecificPokemon.name.ToString();
                txtName.transform.SetParent(ParentPanelInfoSpecific, false);
                txtName.transform.localPosition = new Vector2(0, -40);
                txtName.rectTransform.sizeDelta = new Vector2(241, 50);

                txtHeight = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtHeight.text = "Height: " + dataSpecificPokemon.height.ToString();
                txtHeight.transform.SetParent(ParentPanelInfoSpecific, false);
                txtHeight.transform.localPosition = new Vector2(-81, -105);
                txtHeight.rectTransform.sizeDelta = new Vector2(90, 17);

                txtWeidth = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtWeidth.text = "Weight: " + dataSpecificPokemon.weight.ToString();
                txtWeidth.transform.SetParent(ParentPanelInfoSpecific, false);
                txtWeidth.transform.localPosition = new Vector2(-62, -123);
                txtWeidth.rectTransform.sizeDelta = new Vector2(126, 19);

                txtExperience = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtExperience.text = "Base Experience: " + dataSpecificPokemon.base_experience.ToString();
                txtExperience.transform.SetParent(ParentPanelInfoSpecific, false);
                txtExperience.transform.localPosition = new Vector2(-26, -88);
                txtExperience.rectTransform.sizeDelta = new Vector2(200, 16);

                JObject jo = JObject.Parse(www.downloadHandler.text);
                string ability1 = jo["abilities"][0]["ability"]["name"].Value<string>();
                string ability2 = jo["abilities"][1]["ability"]["name"].Value<string>();

                txtability1 = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtability1.text = ability1;
                txtability1.transform.SetParent(ParentPanelInfoSpecific, false);
                txtability1.transform.localPosition = new Vector2(-67, -226);
                txtability1.rectTransform.sizeDelta = new Vector2(130, 25);

                txtability2 = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtability2.text = ability2;
                txtability2.transform.SetParent(ParentPanelInfoSpecific, false);
                txtability2.transform.localPosition = new Vector2(72, -226);
                txtability2.rectTransform.sizeDelta = new Vector2(130, 25);

                idPokemon = dataSpecificPokemon.id.ToString();

                StartCoroutine(GetLocationPokemon());
                StartCoroutine(GetOneImgPokemon());

            }
        }
    }


    public IEnumerator GetLocationPokemon()
    {
        //
        UnityWebRequest www = UnityWebRequest.Get("https://pokeapi.co/api/v2/pokemon/1/encounters");
        //UnityWebRequest www = UnityWebRequest.Get(locationPokemon);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Algo salio mal");
        }
        else
        {
            if (www.isDone)
            {
                string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                Debug.Log("Json Result pokemon: " + jsonResult);

                JArray ja = JArray.Parse(www.downloadHandler.text);
                string namelocationPokemon = ja[0]["location_area"]["name"].Value<string>();
                 Debug.Log("nombre de ubicacion: " + namelocationPokemon);

                //  JObject jo = JObject.Parse(www.downloadHandler.text);
                //  locationPokemon = jo["location_area"].Value<string>();
                txtLocation = Instantiate(Resources.Load("TextInfo", typeof(Text))) as Text;
                txtLocation.text = "Location: " + namelocationPokemon;
                txtLocation.transform.SetParent(ParentPanelInfoSpecific, false);
                txtLocation.transform.localPosition = new Vector2(1, -145);
                txtLocation.rectTransform.sizeDelta = new Vector2(254, 25);
            }
        }
    }

    public void NextList10()
    {
        DestroyAllButtons();
        index += 10;
        offset = index.ToString();
        StartCoroutine(GetPokemons());
        DisableAllButtons();
        Invoke("EnableAllButtons", 2f);
    }

    public void BackList10()
    {
        if (index == 0)
        {
            Debug.Log("no hay de este lado");
        }
        else
        {
            DestroyAllButtons();
            index -= 10;
            offset = index.ToString();
            StartCoroutine(GetPokemons());
            DisableAllButtons();
            Invoke("EnableAllButtons", 2f);
        }

    }


    public void DestroyAllButtons()
    {
        Debug.Log("Cantidad a destruir :" + cantDestroy);
        for (int i = 0; i < cantDestroy; i++)
        {
            Destroy(goButtonPokemons[i].gameObject);
        }
        cantDestroy = 0;
    }

    
    public void ButtonClicked(string _urlPokemon) //le agrego un listener dynamico al boton que se le agrega a c/u del os botones enlistados
    {
        specificPokemon = _urlPokemon;
        Debug.Log("Button clicked = " + specificPokemon);
        pokedex.SetActive(false);
        panelDataPokemons.SetActive(true);
        StartCoroutine(GetDataSpecificPokemon());

    }

    public void CloseDataPokemons()
    {
        pokedex.SetActive(true);
        panelDataPokemons.SetActive(false);
        //clean vars
        imgSpecific.texture = null;
        imgSpecific.GetComponent<RawImage>().enabled = false;
        Destroy(txtId.gameObject);
        Destroy(txtName.gameObject);
        Destroy(txtHeight.gameObject);
        Destroy(txtWeidth.gameObject);
        Destroy(txtExperience.gameObject);
        Destroy(txtability1.gameObject);
        Destroy(txtability2.gameObject);
        Destroy(txtLocation.gameObject);
    }


    public void OpenPokedex()
    {
        pokedex.SetActive(true);
        btnPokedex.SetActive(false);

        offset = "0";
        StartCoroutine(GetPokemons());

    }

    public void ClosePokedex()
    {
        index = 0;
        offset = index.ToString();
        DestroyAllButtons();

        pokedex.SetActive(false);
        btnPokedex.SetActive(true);
    }


    public void DisableAllButtons()
    {
        //disable all buttons to wait 2 seconds for the api services
        buttonRight.enabled = false;
        buttonLeft.enabled = false;
        backList.enabled = false;
        backSpecific.enabled = false;
}

    public void EnableAllButtons()
    {
        //Enable all buttons 2 seconds for the api services again
        buttonRight.enabled = true;
        buttonLeft.enabled = true;
        backList.enabled = true;
        backSpecific.enabled = true;
    }


}


