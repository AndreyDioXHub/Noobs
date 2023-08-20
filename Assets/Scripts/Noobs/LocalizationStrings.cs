using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationStrings 
{
    public static string game_scene_not_enough_money_curent = "�� ���������� �����\n<size=24>���������� ������� ��� ��������� �� �� �����</size>";
    public static string game_scene_wait_connection_curent = "�������� �������";
    public static string game_scene_lose_curent = "�� ���������!";
    public static string game_scene_lvl_win_t0_curent = "������� �������!";
    public static string game_scene_lvl_win_t1_curent = "�������: ";
    public static string game_scene_game_win_t0_curent = "�� ��������!";
    public static string game_scene_game_win_t1_curent = "�������������� �������: ";
    public static string game_scene_game_win_t2_curent = "������� ����� ������";
    public static string game_scene_game_sencity_curent = "���������������� ����";
    public static string game_scene_wait_curent = "��������";
    public static string game_scene_select_curent = "�����";
    public static string game_scene_prepare_curent = "����������";
    public static string start_scene_play_button_curent = "������";
    public static string start_scene_next_button_curent = "���������";
    public static string start_scene_prev_button_curent = "����������";

    public static string game_scene_not_enough_money_ru = "������������ �����\n<size=24>���������� ������� ��� ��������� �� �����</size>";
    public static string game_scene_wait_connection_ru = "�������� �������";
    public static string game_scene_lose_ru = "�� ���������!";
    public static string game_scene_lvl_win_t0_ru = "������� �������!";
    public static string game_scene_lvl_win_t1_ru = "�������: ";
    public static string game_scene_game_win_t0_ru = "�� ��������!";
    public static string game_scene_game_win_t1_ru = "�������������� �������: ";
    public static string game_scene_game_win_t2_ru = "������� ����� ������";
    public static string game_scene_game_sencity_ru = "���������������� ����";
    public static string game_scene_wait_ru = "��������";
    public static string game_scene_select_ru = "�����";
    public static string game_scene_prepare_ru = "����������";
    public static string start_scene_play_button_ru = "������";
    public static string start_scene_next_button_ru = "���������";
    public static string start_scene_prev_button_ru = "����������";

    public static string game_scene_not_enough_money_en = "Not enough money\n<size=24>Watch ads or get them by playing</size>";
    public static string game_scene_wait_connection_en = "Waiting for the players";
    public static string game_scene_lose_en = "You lost!";
    public static string game_scene_lvl_win_t0_en = "Level passed!";
    public static string game_scene_lvl_win_t1_en = "Reward:";
    public static string game_scene_game_win_t0_en = "You won!";
    public static string game_scene_game_win_t1_en = "Additional reward: ";
    public static string game_scene_game_win_t2_en = "Press eny key";
    public static string game_scene_game_sencity_en = "Mouse sensitivity";
    public static string game_scene_wait_en = "Waiting";
    public static string game_scene_select_en = "Choice";
    public static string game_scene_prepare_en = "Prepare";
    public static string start_scene_play_button_en = "Play";
    public static string start_scene_next_button_en = "next";
    public static string start_scene_prev_button_en = "previous";

    public static bool first_start = false;

    public static void SetLanguage(string local)
    {
        switch (local)
        {
            case "ru":
                game_scene_not_enough_money_curent = game_scene_not_enough_money_ru;
                game_scene_wait_connection_curent = game_scene_wait_connection_ru;
                game_scene_lose_curent = game_scene_lose_ru;
                game_scene_lvl_win_t0_curent = game_scene_lvl_win_t0_ru;
                game_scene_lvl_win_t1_curent = game_scene_lvl_win_t1_ru;
                game_scene_game_win_t0_curent = game_scene_game_win_t0_ru;
                game_scene_game_win_t1_curent = game_scene_game_win_t1_ru;
                game_scene_game_win_t2_curent = game_scene_game_win_t2_ru;
                game_scene_game_sencity_curent = game_scene_game_sencity_ru;

                game_scene_wait_curent = game_scene_wait_ru;
                game_scene_select_curent = game_scene_select_ru;
                game_scene_prepare_curent = game_scene_prepare_ru;

                start_scene_play_button_curent = start_scene_play_button_ru;
                start_scene_next_button_curent = start_scene_next_button_ru;
                start_scene_prev_button_curent = start_scene_prev_button_ru;
                break;
            case "en":
                game_scene_not_enough_money_curent = game_scene_not_enough_money_en;
                game_scene_wait_connection_curent = game_scene_wait_connection_en;
                game_scene_lose_curent = game_scene_lose_en;
                game_scene_lvl_win_t0_curent = game_scene_lvl_win_t0_en;
                game_scene_lvl_win_t1_curent = game_scene_lvl_win_t1_en;
                game_scene_game_win_t0_curent = game_scene_game_win_t0_en;
                game_scene_game_win_t1_curent = game_scene_game_win_t1_en;
                game_scene_game_win_t2_curent = game_scene_game_win_t2_en;
                game_scene_game_sencity_curent = game_scene_game_sencity_en;

                game_scene_wait_curent = game_scene_wait_en;
                game_scene_select_curent = game_scene_select_en;
                game_scene_prepare_curent = game_scene_prepare_en;

                start_scene_play_button_curent = start_scene_play_button_en;
                start_scene_next_button_curent = start_scene_next_button_en;
                start_scene_prev_button_curent = start_scene_prev_button_en;
                break;
            default:
                game_scene_not_enough_money_curent = game_scene_not_enough_money_ru;
                game_scene_wait_connection_curent = game_scene_wait_connection_ru;
                game_scene_lose_curent = game_scene_lose_ru;
                game_scene_lvl_win_t0_curent = game_scene_lvl_win_t0_ru;
                game_scene_lvl_win_t1_curent = game_scene_lvl_win_t1_ru;
                game_scene_game_win_t0_curent = game_scene_game_win_t0_ru;
                game_scene_game_win_t1_curent = game_scene_game_win_t1_ru;
                game_scene_game_win_t2_curent = game_scene_game_win_t2_ru;
                game_scene_game_sencity_curent = game_scene_game_sencity_ru;

                game_scene_wait_curent = game_scene_wait_ru;
                game_scene_select_curent = game_scene_select_ru;
                game_scene_prepare_curent = game_scene_prepare_ru;

                start_scene_play_button_curent = start_scene_play_button_ru;
                start_scene_next_button_curent = start_scene_next_button_ru;
                start_scene_prev_button_curent = start_scene_prev_button_ru;
                break;
        }
    }

}
