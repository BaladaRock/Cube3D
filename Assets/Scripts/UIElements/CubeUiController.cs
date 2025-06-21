using Assets.Scripts.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UIElements
{
    public class CubeUiController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text txtSolution;
        [SerializeField] private Button btnSolve;

        private CubeSolverBridge _solver;
        private CubeManager _cubeMgr;

        //void Awake() => btnSolve.onClick.AddListener(OnSolveClicked);

        private void Awake()
        {
            _solver = FindFirstObjectByType<CubeSolverBridge>();
            _cubeMgr = FindFirstObjectByType<CubeManager>();

            btnSolve.onClick.AddListener(OnSolveClicked);
        }

        private void OnSolveClicked()
        {
            //if (solver == null || cubeMgr == null) return;

            //string state = cubeMgr.GetKociembaState();

            //string solution = solver.FindSolution(state);

            //txtSolution.text = FormatMoves(solution);

            //cubeMgr.PlaySolution(solution);
            txtSolution.text = "R U R' F R2 U'";

        }

        private static string FormatMoves(string raw) =>
            raw.Replace(" ", "\u2009"); // thin space for esthetic spacing
    }
}