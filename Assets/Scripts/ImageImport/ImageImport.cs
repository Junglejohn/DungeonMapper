using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;
using UnityEngine.UI;


public class ImageImport : MonoBehaviour
{
    Texture2D texture = null;
    DropInfo dropInfo = null;
    class DropInfo
    {
        public string file;
        public Vector2 pos;
    }
    void OnEnable ()
    {
        isShowAddTileButton();
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += OnFiles;

        dropRectTextAnim.SetBool("IsEmpty", false);

    }
    void OnDisable()
    {
        UnityDragAndDropHook.UninstallHook();
    }

    void OnFiles(List<string> aFiles, POINT aPos)
    {

        string file = "";
        // scan through dropped files and filter out supported image types
        foreach(var f in aFiles)
        {
            var fi = new System.IO.FileInfo(f);
            var ext = fi.Extension.ToLower();
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
            {
                file = f;
                break;
            }
        }

        // If the user dropped a supported file, create a DropInfo
        if (file != "")
        {
            var info = new DropInfo
            {
                file = file,
                pos = new Vector2(aPos.x, aPos.y)
            };
            dropInfo = info;
            
            LoadImage(dropInfo);
        }

        
    }

    void LoadImage(DropInfo aInfo)
    {
        Debug.LogError("loadImage");

        if (aInfo == null)
        {
            Debug.LogError("dropInfoWasNull");
            return;
        }
            
        // get the GUI rect of the last Label / box
        var rect = dropRectImage.rectTransform.rect;

        //var rect = GUILayoutUtility.GetLastRect();
        // check if the drop position is inside that rect

        var data = System.IO.File.ReadAllBytes(aInfo.file);
        var tex = new Texture2D(1, 1);
        tex.LoadImage(data);
        if (texture != null)
            Destroy(texture);
        texture = tex;

        Debug.LogError("CreatingSpriteFromTexture");
        dropRectImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

        dropRectTextAnim.SetBool("IsEmpty", true);

        isShowAddTileButton();
    
    }

    public Image dropRectImage;
    public InputField namingInputField;
    public Button AddTileButton;
    private bool isShowAddTileButton () {

            if (AddTileButton != null && namingInputField != null && !string.IsNullOrEmpty(namingInputField.text) && dropRectImage.sprite != null)
            {
            AddTileButton.gameObject.SetActive(true);
                return true;
            } else
            {
            AddTileButton.gameObject.SetActive(false);
            return false;
            }
    }
    
    public void OnNamingInputFieldValueChanged()
    {
        if (namingInputField != null)
        {
            isShowAddTileButton();
        } else
        {
            Debug.LogWarning("Naming input field is null");
        }
    }
    
    public Animator dropRectTextAnim;
    private void TriggerImageImportfeedback(bool isSuccess)
    {
        if (dropRectTextAnim != null)
        {
            if (isSuccess)
            {
                dropRectTextAnim.SetTrigger("Success");
            } else
            {
                dropRectTextAnim.SetTrigger("Failure");
            }

            
        } else
        {
            Debug.LogError("Animator for dropRectAnimation feedback is not assigned - assign to display feedback");
        }
    }

    public void AddTileIcon()
    {
        if (Manager.currentManager != null && Manager.currentManager.session.AddLoadSprite(namingInputField.text, dropRectImage.sprite, EventManager.IsForegroundActive))
        {
            Debug.LogError("Added sprite by the name of " + namingInputField.text);

            TriggerImageImportfeedback(true);

        }
        else
        {
            TriggerImageImportfeedback(false);
            Debug.LogWarning("Could not add sprite. Make sure it has a name and a sprite that is not null");
        }

        if(dropRectTextAnim != null)
        {
            dropRectTextAnim.SetBool("IsEmpty", false);
        }
        

        dropRectImage.sprite = null;
        namingInputField.text = "";

    }

}
