
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;


public class Unit : MonoBehaviour
{
    public Fraction fraction;
    Node goal;
    Fieldcolor fieldcolor;
    public float degree;
    public bool isLastUnit; // 5番目の玉なのか?
    float speed = 0.2f;
    private float interpolateAmount;
    Vector3 startPos;
    public int senderID; //攻撃主のID,オブジェクトの受け渡しにつかう
    //float Vecx;






    public void SetUnit(Fraction _fraction, Node goalNode, Material mat, Fieldcolor _fieldcolor, float _degree)
    {
        fraction = _fraction;
        goal = goalNode;
        GetComponent<SpriteRenderer>().material = mat;
        fieldcolor = _fieldcolor;
        //追加　子オブジェクトのマテリアル変更
        //transform.GetChild(1).GetComponent<Renderer>().material = child;
        startPos = transform.position;
        degree = _degree;
    }

    void Update()
    {
        //Vecx = UnityEngine.Random.Range(-0.5f,-0.2f);
        //startPos.x = startPos.x + Vecx;
        double x = 1 - (Math.Pow((goal.transform.position.x - transform.position.x), 2) + Math.Pow((goal.transform.position.y - transform.position.y), 2)) / (Math.Pow((goal.transform.position.x - startPos.x), 2) + Math.Pow((goal.transform.position.y - startPos.y), 2));

        Sprite fieldcolorDifference = fieldcolor.GetComponent<SpriteRenderer>().sprite;

        //Debug.Log(fieldcolorDifference.name);
        //if (fieldcolorDifference.name == "東京")
        //{
        //    Debug.Log("発射東京");
        //    speed = 0.5f;
        //}
        //else
        //{
        //    Debug.Log("発射他");

        //    speed = easeInExpo((float)x);
            
        //}

        switch(fieldcolorDifference.name)
        {
            case "栃木":
                Debug.Log("栃木");
                speed = 0.5f;
                break;

            case "東京":
                Debug.Log("東京");
                speed = easeInExpo((float)x);
                break;

            case "茨城":
                Debug.Log("茨城");
                speed = (float)easeOutElastic((float)x);
                break;

            case "埼玉":
                Debug.Log("埼玉");
                speed = 4f;
                break;
            
            

        }




        if (speed < 0.1f)
        {
            speed = 0.1f;
        }
        else if (speed > 5f)
        {
            speed = 2f;
        }
        interpolateAmount = (interpolateAmount + Time.deltaTime * speed) % 1f;

        transform.position = QuadraticLeap(startPos, goal.transform.position, interpolateAmount);

        //photonView.RPC("QuadraticLeap", RpcTarget.All, startPos, goal.transform.position, interpolateAmount); 
            
        
        
    }
    

        /*private Vector3 QuadraticLeapBefore(Vector3 a, Vector3 c, float t)
        {
        Vector3 b = new Vector3(c.x - 2, c.y - 1, 0);
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, interpolateAmount);}*/
    
    private Vector3 QuadraticLeap(Vector3 a, Vector3 c, float t)
    {
        Vector2 dt = c - a;
        float alpha = Mathf.Atan2(dt.y, dt.x);
        float length = dt.magnitude / 2;
        float bx = length * Mathf.Cos(alpha + degree) + a.x;
        float by = length * Mathf.Sin(alpha + degree) + a.y;

        Vector3 b = new Vector3(bx, by, 0);
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, interpolateAmount);
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
       Node n = col.GetComponent<Node>();
       if(n == goal)
       {
        Debug.Log("敵にあたった");
        n.HandleIncomingUnit(fraction,senderID, fieldcolor);
        Destroy(this.gameObject);
       } 
    }
    float easeInExpo(float x)
    {
        return x == 0f ? 0f : (float)(Math.Pow(0.1f, 20f * x - 0.01f));
    }

    double easeOutElastic(double x) 
    {
        double z = (2 * Math.PI) / 3;

        return x == 0
        ? 0
        : x == 1
        ? 1
        : Math.Pow(2, -10 * x) * Math.Sin(x* 10 - 0.75) * z +1 ;
    }

}
