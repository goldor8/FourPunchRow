using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Puissance4 : MonoBehaviour
{
    int[,] board = new int[6, 7];
    int player = 1;
    int winner = 0;

    [SerializeField] private GameObject blueToken;
    [SerializeField] private GameObject redToken;
    [SerializeField] private GameObject blueTokenPrev;
    [SerializeField] private GameObject redTokenPrev;
    
    void DisplayBoard()
    {
        string tableauStr = "";
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                tableauStr += board[i, j] + " ";
            }
            tableauStr += "\n";
        }
        Debug.Log(tableauStr);
    }

    void Play(int column)
    {

        // remplace le jeton de prévisualisation par le jeton du joueur
        for (int i = 5; i >= 0; i--)
        {
            if (board[i, column] == player + 2)
            {
                board[i, column] = player ;
                Destroy(transform.GetChild(transform.childCount - 1).GameObject());
                GameObject token;
                token = player == 1 ? blueToken: redToken;
                GameObject obj = Instantiate(token, transform);
                Vector3 position = new Vector3(-0.4983f + 0.1673f * column, 0.0011f, -0.3237f + 0.1327f * (5-i));
                Quaternion rotation = Quaternion.Euler(0, -90, 0);
                obj.transform.localPosition = position;
                obj.transform.localRotation = rotation;
                player = player == 1 ? 2 : 1; 
                return;
            }
        }
        
        // supprime le jeton de prévisualisation
        for (int col = 0; col < 7; col++) {
            for (int i = 5; i >= 0; i--) {
                if (board[i, col] == 3 || board[i, col] == 4) {
                    Debug.Log("Remove token at " + i + " " + col);
                    board[i, col] = 0;
                    Destroy(transform.GetChild(transform.childCount - 1).GameObject());
                    break;
                }
            }
        }

        // place le jeton de prévisualisation
        for (int i = 5; i >= 0; i--)
        {
            if (board[i, column] == 0)
            {
                board[i, column] = player + 2;
                GameObject token;
                token = player == 1 ? blueTokenPrev: redTokenPrev;
                GameObject obj = Instantiate(token, transform);
                Vector3 position = new Vector3(-0.4983f + 0.1673f * column, 0.0011f, -0.3237f + 0.1327f * (5-i));
                Quaternion rotation = Quaternion.Euler(0, -90, 0);
                obj.transform.localPosition = position;
                obj.transform.localRotation = rotation;
                break;
            }
        }
        Debug.Log("Player " + player + " turn");
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayBoard();
        }
        else
        {
            int column = -1;

            // Détecte la colonne en fonction de la touche pressée
            if (Input.GetKeyDown(KeyCode.Q))
            {
                column = 0;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                column = 1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                column = 2;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                column = 3;
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                column = 4;
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                column = 5;
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                column = 6;
            }

            if (column != -1)
            {
                Play(column);
                DisplayBoard();
            }
        }
    }

    void BotPlaceToken(int column) {

        for (int i = 5; i >= 0; i--) {
            if (board[i, column] == 0) {
                board[i, column] = player;
                GameObject token;
                token = redToken;
                GameObject obj = Instantiate(token, transform);
                Vector3 position = new Vector3(-0.4983f + 0.1673f * column, 0.0011f, -0.3237f + 0.1327f * (5 - i));
                Quaternion rotation = Quaternion.Euler(0, -90, 0);
                obj.transform.localPosition = position;
                obj.transform.localRotation = rotation;
                player = player == 1 ? 2 : 1;
                return;
            }
        }
    }
    
    bool CheckWin()
    {
        // Check horizontal
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] != 0 && board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2] && board[i, j] == board[i, j + 3])
                {
                    winner = board[i, j];
                    return true;
                }
            }
        }

        // Check vertical
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (board[i, j] != 0 && board[i, j] == board[i + 1, j] && board[i, j] == board[i + 2, j] && board[i, j] == board[i + 3, j])
                {
                    winner = board[i, j];
                    return true;
                }
            }
        }

        // Check diagonal
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] != 0 && board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2] && board[i, j] == board[i + 3, j + 3])
                {
                    winner = board[i, j];
                    return true;
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 3; j < 7; j++)
            {
                if (board[i, j] != 0 && board[i, j] == board[i + 1, j - 1] && board[i, j] == board[i + 2, j - 2] && board[i, j] == board[i + 3, j - 3])
                {
                    winner = board[i, j];
                    return true;
                }
            }
        }
        return false;
    }

    bool IsColumnPlayable(int column)
    {
        for (int i = 0; i < 6; i++)
        {
            if (board[i, column] == 0)
            {
                return true;
            }
        }
        return false;
    }
    
    void BotPlay()
{
    // Check pour 3 alignements

    // Check horizontal
    for (int i = 0; i < 6; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i, j + 1] && board[i, j] == board[i, j + 2])
            {
                if (board[i, j + 2] == 0) {
                    BotPlaceToken(j + 3);
                    return;
                }
            }
        }
    }

    // Check vertical
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 7; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i + 1, j] && board[i, j] == board[i + 2, j])
            {
                if ( j == 0 || board[i + 3, j - 1] == 0) {
                    BotPlaceToken(j);
                    return;
                }
                
            }
        }
    }

    // Check diagonal
    for (int i = 0; i < 3; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i + 1, j + 1] && board[i, j] == board[i + 2, j + 2])
            {
                BotPlaceToken(j + 3);
                return;
            }
        }
    }

    // Check antidiagonal
    for (int i = 0; i < 3; i++)
    {
        for (int j = 3; j < 7; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i + 1, j - 1] && board[i, j] == board[i + 2, j - 2])
            {
                BotPlaceToken(j - 3);
                return;
            }
        }
    }

    // Check pour 2 alignements

    // Check horizontal
    for (int i = 0; i < 6; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i, j + 1])
            {
                if (j + 2 < 7 && board[i, j + 2] == 0)
                {
                    BotPlaceToken(j + 2);
                    return;
                }
                if (j - 1 >= 0 && board[i, j - 1] == 0)
                {
                    BotPlaceToken(j - 1);
                    return;
                }
            }
        }
    }

    // Check vertical
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 7; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i + 1, j])
            {
                if (i + 2 < 6 && board[i + 2, j] == 0)
                {
                    BotPlaceToken(j);
                    return;
                }
            }
        }
    }

    // Check diagonal
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i + 1, j + 1])
            {
                if (i + 2 < 6 && j + 2 < 7 && board[i + 2, j + 2] == 0)
                {
                    BotPlaceToken(j + 2);
                    return;
                }
                if (i - 1 >= 0 && j - 1 >= 0 && board[i - 1, j - 1] == 0)
                {
                    BotPlaceToken(j - 1);
                    return;
                }
            }
        }
    }

    // Check antidiagonal
    for (int i = 0; i < 4; i++)
    {
        for (int j = 2; j < 7; j++)
        {
            if (board[i, j] != 0 && board[i, j] == board[i + 1, j - 1])
            {
                if (i + 2 < 6 && j - 2 >= 0 && board[i + 2, j - 2] == 0)
                {
                    BotPlaceToken(j - 2);
                    return;
                }
                if (i - 1 >= 0 && j + 1 < 7 && board[i - 1, j + 1] == 0)
                {
                    BotPlaceToken(j + 1);
                    return;
                }
            }
        }
    }

    // Si aucun coup stratégique n'est trouvé, jouer un coup aléatoire
    int randomColumn = Random.Range(0, 6);
    while (board[0, randomColumn] != 0)
    {
        randomColumn = Random.Range(0, 6);
    }
    BotPlaceToken(randomColumn);
}







    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hello, World!");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                board[i, j] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (winner != 0)
        {
            Debug.Log("The game is over !");
        }
        else{
            if (CheckWin())
            {
                Debug.Log("Player " + winner + " wins !");
            }
            else if (Input.anyKeyDown)
            {
                CheckInput();
            }
            else if (player == 2) {
                BotPlay();
            }
        }    

    }
}
