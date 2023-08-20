using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationText : MonoBehaviour
{
    [SerializeField]
    private string _key;

    private TextMeshProUGUI _text;

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    public void UpdateText()
    {
        string text = "";

        switch (_key)
        {
            case "game_scene_not_enough_money_curent":
                text = LocalizationStrings.game_scene_not_enough_money_curent;
                break;
            case "game_scene_wait_connection_curent":
                text = LocalizationStrings.game_scene_wait_connection_curent;
                break;
            case "game_scene_lose_curent":
                text = LocalizationStrings.game_scene_lose_curent;
                break;
            case "game_scene_lvl_win_t0_curent":
                text = LocalizationStrings.game_scene_lvl_win_t0_curent;
                break;
            case "game_scene_lvl_win_t1_curent":
                text = LocalizationStrings.game_scene_lvl_win_t1_curent;
                break;
            case "game_scene_game_win_t0_curent":
                text = LocalizationStrings.game_scene_game_win_t0_curent;
                break;
            case "game_scene_game_win_t1_curent":
                text = LocalizationStrings.game_scene_game_win_t1_curent;
                break;
            case "game_scene_game_win_t2_curent":
                text = LocalizationStrings.game_scene_game_win_t2_curent;
                break;
            case "start_scene_play_button_curent":
                text = LocalizationStrings.start_scene_play_button_curent;
                break;
            case "start_scene_next_button_curent":
                text = LocalizationStrings.start_scene_next_button_curent;
                break;
            case "start_scene_prev_button_curent":
                text = LocalizationStrings.start_scene_prev_button_curent;
                break;
            case "game_scene_game_sencity_curent":
                text = LocalizationStrings.game_scene_game_sencity_curent;
                break;
            case "game_scene_prepare_curent":
                text = LocalizationStrings.game_scene_prepare_curent;
                break;
            case "game_scene_select_curent":
                text = LocalizationStrings.game_scene_select_curent;
                break;
            case "game_scene_wait_curent":
                text = LocalizationStrings.game_scene_wait_curent;
                break;
            default:
                break;
        }

        try
        {
            _text.text = text;
        }
        catch(NullReferenceException e)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
