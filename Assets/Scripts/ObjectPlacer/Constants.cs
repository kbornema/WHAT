using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPlacerConstants
{
    //none
    const int B0 = 0;
    //up      
    const int B1 = 1;
    //up left   
    const int B2 = 2;
    //left    
    const int B3 = 4;
    //left down
    const int B4 = 8;
    //down
    const int B5 = 16;
    //down right
    const int B6 = 32;
    //right
    const int B7 = 64;
    //right up
    const int B8 = 128;

    public static int[] TILE_ID_TO_BITS_4 = 
    {
         //0
         B5,
         //1
         B5 | B7,
         //2
         B3 | B5 | B7,
         //3
         B3 | B5,
         //4
         B1 | B5,
         //5
         B1 | B5 | B7,
         //6
         B1 | B3 | B5 | B7,
         //7
         B1 | B3 | B5,
         //8
         B1,
         //9
         B1 | B7,
         //10
         B1 | B3 | B7,
         //11
         B1 | B3, 
         //12
         B0,
         //13
         B7,
         //14
         B3 | B7,
         //15
         B3
     };

    public static int[] TILE_ID_TO_BITS_8 = 
    {
        //0
        B5,
        //1
        B5 | B6 | B7,
        //2
        B3 | B4 | B5 | B6 | B7,
        //3
        B3 | B4 | B5,
        //4
        B5 | B7,
        //5
        B3 | B5 | B7,
        //6
        B3 | B5,
        //7
        B1 | B2 | B3 | B4 | B5 | B7 | B8,
        //8
        B1 | B2 | B3 | B5 | B7 | B8,
        //9
        B1 | B2 | B3 | B5 | B6 | B7 | B8,
        //10
        B1 | B5,
        //11
        B1 | B5 | B6 | B7 | B8,
        //12 all sides:
        B1 | B2 | B3 | B4 | B5 | B6 | B7 | B8,
        //13
        B1 | B2 | B3 | B4 | B5,
        //14
        B1 | B5 | B7,
        //15 no sides:
        B0,
        //16
        B1 | B3 | B5,
        //17
        B1 | B2 | B3 | B4 | B5 | B7,
        //18
        B1 | B3 | B5 | B7,
        //19
        B1 | B3 | B5 | B6 | B7 | B8,
        //20
        B1,
        //21
        B1 | B7 | B8,
        //22
        B1 | B2 | B3 | B7 | B8,
        //23
        B1 | B2 | B3,
        //24
        B1 | B7,
        //25
        B1 | B3 | B7,
        //26
        B1 | B3,
        //27
        B1 | B2 | B3 | B4 | B5 | B6 | B7,
        //28
        B1 | B3 | B4 | B5 | B6 | B7,
        //29
        B1 | B3 | B4 | B5 | B6 | B7 | B8,
        //30
        B3 | B4 | B5 | B7,
        //31:
        B1 | B2 | B3 | B5,
        //32: 
        B1 | B5 | B7 | B8,
        //33:
        B3 | B5 | B6 | B7,
        //34
        B1 | B2 | B3 | B5 | B7,
        //35
        B1 | B3 | B5 | B7 | B8,
        //36
        B1 | B2 | B3 | B5 | B6 | B7,
        //37
        B7,
        //38
        B3 | B7,
        //39
        B3,
        //40
        B1 | B5 | B6 | B7,
        //41
        B1 | B3 | B7 | B8,
        //42
        B1 | B2 | B3 | B7,
        //43
        B1 | B3 | B4 | B5,
        //44
        B1 | B3 | B4 | B5 | B7,
        //45
        B1 | B3 | B5 | B6 | B7,
        //46
        B1 | B3 | B4 | B5 | B7 | B8,
    };
}
