using UnityEngine;
using System.Collections;

public class BaseActivatable : MonoBehaviour {

    public virtual void Start() {
    }

    public virtual void Activate() {
        GameManager.Instance.ActivatedObject = this.gameObject;

    }

    public virtual void Deactivate() {
        GameManager.Instance.DeactivatedObject = this.gameObject;
    }
}
