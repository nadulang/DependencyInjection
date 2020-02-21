using System;
using Npgsql;
using ContactApi2.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

public interface IDatabase
{
    int Create(UserContact user);
    List<UserContact> Read();
    UserContact GetById(int id);
    int Delete(int id);
    int Update([FromBody]JsonPatchDocument<UserContact> user, int id);
}

public class Database : IDatabase
{
    NpgsqlConnection _connection;

    public Database(NpgsqlConnection connection)
    {
        _connection = connection;
        _connection.Open();
    }

    public int Create(UserContact user)
    {
        var command = _connection.CreateCommand();
        command.CommandText = "INSERT INTO contacts (username, passkey, email, full_name) VALUES (@username, @passkey, @email, @full_name) RETURNING id";

        command.Parameters.AddWithValue("@username", user.Username);
        command.Parameters.AddWithValue("@passkey", user.Passkey);
        command.Parameters.AddWithValue("@email", user.Email);
        command.Parameters.AddWithValue("@full_name", user.Full_name);

        command.Prepare();

        var result = command.ExecuteScalar();
        _connection.Close();

        return (int)result;

    }

    public List<UserContact> Read()
    {
        var command = _connection.CreateCommand();
        command.CommandText = "SELECT * FROM contacts";

        var result = command.ExecuteReader();
        var UserContact = new List<UserContact>();
        while (result.Read())
                UserContact.Add(new UserContact() { Id = (int)result[0], Username = (string)result[1], Passkey = (string)result[2], Email = (string)result[3], Full_name = (string)result[4]});
            _connection.Close();
            return UserContact;
    }

    public UserContact GetById(int id)
    {
        var command = _connection.CreateCommand();
        command.CommandText = $"SELECT * FROM contacts WHERE id={id}";
        // command.Parameters.AddWithValue("@id", id);
        var result = command.ExecuteReader();
        result.Read();
        var UserContact = new UserContact()
        { Id = (int)result[0], Username = (string)result[1], Passkey = (string)result[2], Email = (string)result[3], Full_name = (string)result[4] };
            _connection.Close();
        
        return UserContact;

    }

    public int Delete(int id)
    {
        var command = _connection.CreateCommand();

        command.CommandText = $"DELETE FROM contacts WHERE id={id}";

        // command.Parameters.AddWithValue("@id", user.Id);

        var result = command.ExecuteNonQuery();
        _connection.Close();
        return (int)result;
    }

    public int Update([FromBody]JsonPatchDocument<UserContact> user, int id)
    {
        var command = _connection.CreateCommand();
        var users = GetById(id);
        _connection.Open();
        user.ApplyTo(users);

        command.CommandText = $"UPDATE contacts (username, passkey, email, full_name) SET (@username, @passkey, @email, @full_name) WHERE id={id} RETURNING id, username, passkey, email, full_name";
        command.Parameters.AddWithValue("@username", users.Username);
        command.Parameters.AddWithValue("@passkey", users.Passkey);
        command.Parameters.AddWithValue("@email", users.Email);
        command.Parameters.AddWithValue("@full_name", users.Full_name);
        
        var result = command.ExecuteNonQuery();
        _connection.Close();

        return result;

    }
}