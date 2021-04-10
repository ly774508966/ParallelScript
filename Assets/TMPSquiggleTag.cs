using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
 
//Adapted from the TMP vertex jitter example
public class TMPSquiggleTag : MonoBehaviour {
    private TMP_Text m_TextComponent;
    private bool hasTextChanged;
 
    void Awake() {
        m_TextComponent = GetComponent<TMP_Text>();
    }
 
    void OnEnable() {
        // Subscribe to event fired when text object has been regenerated.
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
    }
 
    void OnDisable() {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
    }
 
 
    void Start() {
        StartCoroutine(DoSquiggle());
    }
 
 
    void ON_TEXT_CHANGED(Object obj) {
        if (obj == m_TextComponent)
            hasTextChanged = true;
    }
 
    IEnumerator DoSquiggle () {
        m_TextComponent.ForceMeshUpdate(); //force the mesh to be generated before the end of the frame
       
        TMP_TextInfo textInfo = m_TextComponent.textInfo;
        TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        Matrix4x4 matrix;
        hasTextChanged = true;
        List<int> squiggleChars = new List<int>();
        List<int> hideChars = new List<int>();
 
        while (true) {
            if (hasTextChanged) {
                cachedMeshInfo = textInfo.CopyMeshInfoVertexData(); // Update the copy of the vertex data for the text object.
                //make the squiggle tag characters invisible and record what characters should wiggle
                squiggleChars.Clear();
                hideChars.Clear();
                if (m_TextComponent.text.Contains(@"<squiggle>") && m_TextComponent.text.Contains(@"</squiggle>")) {
                    int beginSearch = 0;
                    for (int i = 0; i < Regex.Matches(m_TextComponent.text,@"<squiggle>").Count; i++) {
                        int beginBracket = m_TextComponent.text.IndexOf(@"<squiggle>",beginSearch);
                        int endBracket = m_TextComponent.text.IndexOf(@"</squiggle>",beginSearch);
                        //hide squiggle tags
                        for (int j = beginBracket; j < beginBracket + 10; j++) hideChars.Add(j);
                        for (int j = endBracket; j < endBracket + 11; j++) hideChars.Add(j);
                        //put down the correct characters to be squiggled
                        for (int c = beginBracket+10; c < endBracket; c++) {
                            squiggleChars.Add(c);
                        }
                        //make sure you dont just keep doing this pair of squiggle tags if there's others to do
                        beginSearch = endBracket;
                    }
                }
                hasTextChanged = false;
            }
           
            int characterCount = textInfo.characterCount;
            if (characterCount == 0) { yield return new WaitForSeconds(0.25f); continue; } //wait for text to be added, if its not already
 
            for (int i = 0; i < characterCount; i++) {
                //hide the squiggle tags
                if (hideChars.Contains(i)) {
                    //get stuff needed for doing things
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
 
                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
 
                    destinationVertices[vertexIndex + 0] = Vector3.zero;
                    destinationVertices[vertexIndex + 1] = Vector3.zero;
                    destinationVertices[vertexIndex + 2] = Vector3.zero;
                    destinationVertices[vertexIndex + 3] = Vector3.zero;
 
                    m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                }
               
                //offset the vertical position by sine(character index in string)
                if (squiggleChars.Contains(i)) {
 
                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
 
                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
 
                    // Get the cached vertices of the mesh used by this text element (character or sprite).
                    Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
 
                    // Determine the center point of each character.
                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
 
                    // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
                    // This is needed so the matrix TRS is applied at the origin for each character.
                    Vector3 offset = charMidBasline;
 
                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
 
                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;
 
                    matrix = Matrix4x4.TRS(new Vector3(0,5*Mathf.Sin(Time.realtimeSinceStartup*i),0),Quaternion.identity, Vector3.one);
 
                    destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
                    destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
                    destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
                    destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);
 
                    destinationVertices[vertexIndex + 0] += offset;
                    destinationVertices[vertexIndex + 1] += offset;
                    destinationVertices[vertexIndex + 2] += offset;
                    destinationVertices[vertexIndex + 3] += offset;
 
                    m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
                }
            }
            /*// Push changes into meshes
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                m_TextComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }*/
           
            yield return new WaitForSeconds(0.05f);
        }
    }
}
