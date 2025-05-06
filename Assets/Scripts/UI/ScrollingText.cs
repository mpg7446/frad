using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScrollingText : CryptidUtils
{
    [SerializeField] private TMP_Text tmp;
    private string text;
    private int length;
    private int pointer = -1;
    private int delay;
    private int textDelay;
    [Range(1,50)]
    public int textSpeed = 2;

    private void Start()
    {
        if (tmp == null)
        {
            try
            {
                tmp = GetComponent<TextMeshPro>();
            } catch
            {
                LogErr("Failed to find TextMeshPro on object " + gameObject.name);
                return;
            }
        }

        text = tmp.text;
        tmp.text = "";
        length = text.Length - 1;

        textDelay = 50 - textSpeed;
        if (textDelay < 0)
            textDelay = 1;
        delay = textDelay;
    }

    private void FixedUpdate()
    {
        if (pointer < length && delay == 0) {
            try {
                char next = getNextChar();
                if (next != '\0')
                    tmp.text += next;
            } catch (Exception e) {
                LogErr("failed to print getNextChar() to TextMeshPro Component: " + e.Message);
            }
        }

        if (delay == 0)
        {
            delay = textDelay;
        }
        else
            delay--;
    }

    private char getNextChar()
    {
        pointer++;
        char next = text[pointer];
        if (next == '/')
        {
            pointer++;
            char arg = text[pointer];
            switch (arg) {
                case 's':
                    waitSeconds();
                    break;
                case 'w':
                    waitClock();
                    break;
            }
            pointer--;
            return '\0';
        }
        return next;
    }

    private void waitSeconds()
    {
        waitClock(50);
    }
    private void waitClock(int mult = 1)
    {
        string input = "";
        pointer ++;
        while (isChar(text[pointer]))
        {
            input += text[pointer];
            pointer++;
        }
        int wait = Int32.Parse(input);
        delay = wait * mult;
        Log("this should wait " + (float)delay/50 + "s");
    }

    private bool isChar(char input)
    {
        int value = toInt(input);
        //Log(value + " isChar from " + input);

        if (value < 0 || value > 10)
            return false;
        return true;
    }

    private int toInt(char input)
    {
        return input - '0';
    }
}
