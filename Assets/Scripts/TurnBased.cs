using UnityEngine;
using System.Collections;

// 1. Alur pertarungan
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
    [Header("Combatants")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    
    // Menyimpan status aktif pertarungan
    private BattleState currentState;

    private void Start()
    {
        currentState = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // 2. Setup Battle
    private IEnumerator SetupBattle()
    {
        Debug.Log("Pertarungan Dimulai! Mempersiapkan arena...");
        yield return new WaitForSeconds(1.5f);

        // Pindah ke giliran pemain setelah persiapan selesai
        PlayerTurn();
    }

    // 3. Giliran Pemain
    private void PlayerTurn()
    {
        currentState = BattleState.PLAYERTURN;
        Debug.Log("Giliran Pemain: Silakan pilih aksi di UI.");
    }

    // Pemicu aksi ketika Tombol "SERANG" di UI diklik oleh pemain
    public void OnAttackButton()
    {
        // Pengaman agar tombol tidak bisa dieksploitasi di luar gilirannya
        if (currentState != BattleState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        Debug.Log("Pemain menyerang musuh!");
        
        yield return new WaitForSeconds(1f); // Jeda animasi serang

        // Cek apakah musuh sudah mati
        bool isEnemyDead = false;

        if (isEnemyDead)
        {
            currentState = BattleState.WON;
            EndBattle();
        }
        else
        {
            // Jika musuh masih hidup, oper giliran ke musuh
            StartCoroutine(EnemyTurn());
        }
    }

    // 4. Giliran Musuh
    private IEnumerator EnemyTurn()
    {
        currentState = BattleState.ENEMYTURN;
        Debug.Log("Giliran Musuh sedang berpikir...");

        yield return new WaitForSeconds(1f); // Jeda berpikir AI

        Debug.Log("Musuh menyerang balik pemain!");
        
        yield return new WaitForSeconds(1f); // Jeda animasi musuh

        bool isPlayerDead = false;

        if (isPlayerDead)
        {
            currentState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            // Jika pemain selamat, kembali ke giliran pemain
            PlayerTurn();
        }
    }

    // 5. Fase Penyelesaian
    private void EndBattle()
    {
        if (currentState == BattleState.WON)
        {
            Debug.Log("Anda Menang! Mendapatkan EXP dan Gold.");
            // Kembali ke World Map Scene
        }
        else if (currentState == BattleState.LOST)
        {
            Debug.Log("Anda Kalah! Game Over.");
            // Pindah ke layar Game Over
        }
    }
}