using Assets.Scripts.DataBase;
using TMPro;
using UnityEngine;

public class Registration : MonoBehaviour
{
    [SerializeField] private FirebaseDbContext _firebaseDbContext;
    [SerializeField] private PlayerRepository _playerRepository;

    [SerializeField] private GameObject _gameManager;

    [SerializeField] private TMP_InputField _name;
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_InputField _confirmPassword;

    [SerializeField] private TMP_Text _errorMessage;

    public async void onClickRegisterButton()
    {
        if (!CheckFields()) return;

        var newPlayer = new Player(_name.text, _email.text, _password.text, new Score());
        var check = await _firebaseDbContext.AddUserAsync(newPlayer);

        if (!check) _errorMessage.text = "Such a user exists";
        else
        {
            _playerRepository.Initialize(newPlayer);
            gameObject.SetActive(false);
            _gameManager.SetActive(true);
        }
    }

    private bool CheckFields()
    {
        if (!Helper.CheckInternetConnection())
        {
            _errorMessage.text = "No internet connection";
            return false;
        }

        if (!_name.text.Equals("") & !_email.text.Equals("") & !_password.text.Equals("") & !_confirmPassword.text.Equals(""))
        {
            if (_password.text.Equals(_confirmPassword.text))
            {
                _errorMessage.text = "";
                return true;
            }

            else _errorMessage.text = "Password and password confirmation do not match";
        }
        else _errorMessage.text = "Fill in all the fields";

        return false;
    }
}