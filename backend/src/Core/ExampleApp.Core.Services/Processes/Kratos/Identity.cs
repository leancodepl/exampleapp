#nullable enable

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LeanCode.Kratos.Client.Model;

/// <summary>
/// An [identity](https://www.ory.sh/docs/kratos/concepts/identity-user-model) represents a (human) user in Ory.
/// </summary>
public class Identity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Identity" /> class.
    /// </summary>
    /// <param name="createdAt">CreatedAt is a helper struct field for gobuffalo.pop.</param>
    /// <param name="id">ID is the identity&#39;s unique identifier.  The Identity ID can not be changed and can not be chosen. This ensures future compatibility and optimization for distributed stores such as CockroachDB.</param>
    /// <param name="recoveryAddresses">RecoveryAddresses contains all the addresses that can be used to recover an identity.</param>
    /// <param name="schemaId">SchemaID is the ID of the JSON Schema to be used for validating the identity&#39;s traits.</param>
    /// <param name="schemaUrl">SchemaURL is the URL of the endpoint where the identity&#39;s traits schema can be fetched from.  format: url</param>
    /// <param name="state">state</param>
    /// <param name="stateChangedAt">stateChangedAt</param>
    /// <param name="updatedAt">UpdatedAt is a helper struct field for gobuffalo.pop.</param>
    /// <param name="verifiableAddresses">VerifiableAddresses contains all the addresses that can be verified by the user.</param>
    /// <param name="metadataAdmin">NullJSONRawMessage represents a json.RawMessage that works well with JSON, SQL, and Swagger and is NULLable-</param>
    /// <param name="metadataPublic">NullJSONRawMessage represents a json.RawMessage that works well with JSON, SQL, and Swagger and is NULLable-</param>
    /// <param name="traits">Traits represent an identity&#39;s traits. The identity is able to create, modify, and delete traits in a self-service manner. The input will always be validated against the JSON Schema defined in &#x60;schema_url&#x60;.</param>
    [JsonConstructor]
    public Identity(
        DateTime createdAt,
        Guid id,
        List<RecoveryIdentityAddress> recoveryAddresses,
        string schemaId,
        string schemaUrl,
        IdentityState state,
        DateTime stateChangedAt,
        DateTime updatedAt,
        List<VerifiableIdentityAddress> verifiableAddresses,
        JsonElement? metadataAdmin = default,
        JsonElement? metadataPublic = default,
        JsonElement traits = default
    )
    {
        CreatedAt = createdAt;
        Id = id;
        RecoveryAddresses = recoveryAddresses;
        SchemaId = schemaId;
        SchemaUrl = schemaUrl;
        State = state;
        StateChangedAt = stateChangedAt;
        UpdatedAt = updatedAt;
        VerifiableAddresses = verifiableAddresses;
        MetadataAdmin = metadataAdmin;
        MetadataPublic = metadataPublic;
        Traits = traits;
    }

    /// <summary>
    /// Gets or Sets State
    /// </summary>
    [JsonPropertyName("state")]
    public IdentityState State { get; set; }

    /// <summary>
    /// CreatedAt is a helper struct field for gobuffalo.pop.
    /// </summary>
    /// <value>CreatedAt is a helper struct field for gobuffalo.pop.</value>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// ID is the identity&#39;s unique identifier.  The Identity ID can not be changed and can not be chosen. This ensures future compatibility and optimization for distributed stores such as CockroachDB.
    /// </summary>
    /// <value>ID is the identity&#39;s unique identifier.  The Identity ID can not be changed and can not be chosen. This ensures future compatibility and optimization for distributed stores such as CockroachDB.</value>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// RecoveryAddresses contains all the addresses that can be used to recover an identity.
    /// </summary>
    /// <value>RecoveryAddresses contains all the addresses that can be used to recover an identity.</value>
    [JsonPropertyName("recovery_addresses")]
    public List<RecoveryIdentityAddress> RecoveryAddresses { get; set; }

    /// <summary>
    /// SchemaID is the ID of the JSON Schema to be used for validating the identity&#39;s traits.
    /// </summary>
    /// <value>SchemaID is the ID of the JSON Schema to be used for validating the identity&#39;s traits.</value>
    [JsonPropertyName("schema_id")]
    public string SchemaId { get; set; }

    /// <summary>
    /// SchemaURL is the URL of the endpoint where the identity&#39;s traits schema can be fetched from.  format: url
    /// </summary>
    /// <value>SchemaURL is the URL of the endpoint where the identity&#39;s traits schema can be fetched from.  format: url</value>
    [JsonPropertyName("schema_url")]
    public string SchemaUrl { get; set; }

    /// <summary>
    /// Gets or Sets StateChangedAt
    /// </summary>
    [JsonPropertyName("state_changed_at")]
    public DateTime StateChangedAt { get; set; }

    /// <summary>
    /// UpdatedAt is a helper struct field for gobuffalo.pop.
    /// </summary>
    /// <value>UpdatedAt is a helper struct field for gobuffalo.pop.</value>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// VerifiableAddresses contains all the addresses that can be verified by the user.
    /// </summary>
    /// <value>VerifiableAddresses contains all the addresses that can be verified by the user.</value>
    [JsonPropertyName("verifiable_addresses")]
    public List<VerifiableIdentityAddress> VerifiableAddresses { get; set; }

    /// <summary>
    /// NullJSONRawMessage represents a json.RawMessage that works well with JSON, SQL, and Swagger and is NULLable-
    /// </summary>
    /// <value>NullJSONRawMessage represents a json.RawMessage that works well with JSON, SQL, and Swagger and is NULLable-</value>
    [JsonPropertyName("metadata_admin")]
    public JsonElement? MetadataAdmin { get; set; }

    /// <summary>
    /// NullJSONRawMessage represents a json.RawMessage that works well with JSON, SQL, and Swagger and is NULLable-
    /// </summary>
    /// <value>NullJSONRawMessage represents a json.RawMessage that works well with JSON, SQL, and Swagger and is NULLable-</value>
    [JsonPropertyName("metadata_public")]
    public JsonElement? MetadataPublic { get; set; }

    /// <summary>
    /// Traits represent an identity&#39;s traits. The identity is able to create, modify, and delete traits in a self-service manner. The input will always be validated against the JSON Schema defined in &#x60;schema_url&#x60;.
    /// </summary>
    /// <value>Traits represent an identity&#39;s traits. The identity is able to create, modify, and delete traits in a self-service manner. The input will always be validated against the JSON Schema defined in &#x60;schema_url&#x60;.</value>
    [JsonPropertyName("traits")]
    public JsonElement Traits { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class Identity {\n");
        sb.Append("  CreatedAt: ").Append(CreatedAt).Append('\n');
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  RecoveryAddresses: ").Append(RecoveryAddresses).Append('\n');
        sb.Append("  SchemaId: ").Append(SchemaId).Append('\n');
        sb.Append("  SchemaUrl: ").Append(SchemaUrl).Append('\n');
        sb.Append("  State: ").Append(State).Append('\n');
        sb.Append("  StateChangedAt: ").Append(StateChangedAt).Append('\n');
        sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append('\n');
        sb.Append("  VerifiableAddresses: ").Append(VerifiableAddresses).Append('\n');
        sb.Append("  MetadataAdmin: ").Append(MetadataAdmin).Append('\n');
        sb.Append("  MetadataPublic: ").Append(MetadataPublic).Append('\n');
        sb.Append("  Traits: ").Append(Traits).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }
}

/// <summary>
/// The state can either be &#x60;active&#x60; or &#x60;inactive&#x60;.
/// </summary>
/// <value>The state can either be &#x60;active&#x60; or &#x60;inactive&#x60;.</value>
[JsonConverter(typeof(IdentityStateConverter))]
public enum IdentityState
{
    /// <summary>
    /// Enum Active for value: active
    /// </summary>
    Active = 1,

    /// <summary>
    /// Enum Inactive for value: inactive
    /// </summary>
    Inactive = 2
}

public class IdentityStateConverter : JsonConverter<IdentityState>
{
    public static IdentityState FromString(string value)
    {
        return value switch
        {
            "active" => IdentityState.Active,
            "inactive" => IdentityState.Inactive,
            _ => throw new NotImplementedException($"Could not convert value to type IdentityState: '{value}'")
        };
    }

    public static IdentityState? FromStringOrDefault(string? value)
    {
        return value switch
        {
            "active" => IdentityState.Active,
            "inactive" => IdentityState.Inactive,
            _ => null
        };
    }

    public static string ToJsonValue(IdentityState value)
    {
        return value switch
        {
            IdentityState.Active => "active",
            IdentityState.Inactive => "inactive",
            _ => throw new NotImplementedException($"Value could not be handled: '{value}'")
        };
    }

    public override bool HandleNull => false;

    /// <summary>
    /// Returns a IdentityState from the Json object
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override IdentityState Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return FromStringOrDefault(reader.GetString()) ?? throw new JsonException();
    }

    /// <summary>
    /// Writes the IdentityState to the json writer
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="identityState"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, IdentityState identityState, JsonSerializerOptions options)
    {
        writer.WriteStringValue(ToJsonValue(identityState));
    }
}

/// <summary>
/// VerifiableAddress is an identity&#39;s verifiable address
/// </summary>
public class VerifiableIdentityAddress
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifiableIdentityAddress" /> class.
    /// </summary>
    /// <param name="createdAt">When this entry was created</param>
    /// <param name="id">The ID</param>
    /// <param name="status">VerifiableAddressStatus must not exceed 16 characters as that is the limitation in the SQL Schema</param>
    /// <param name="updatedAt">When this entry was last updated</param>
    /// <param name="value">The address value  example foo@user.com</param>
    /// <param name="verified">Indicates if the address has already been verified</param>
    /// <param name="verifiedAt">verifiedAt</param>
    /// <param name="via">VerifiableAddressType must not exceed 16 characters as that is the limitation in the SQL Schema</param>
    [JsonConstructor]
    public VerifiableIdentityAddress(
        DateTime createdAt,
        Guid id,
        string status,
        DateTime updatedAt,
        string value,
        bool verified,
        DateTime? verifiedAt,
        string via
    )
    {
        CreatedAt = createdAt;
        Id = id;
        Status = status;
        UpdatedAt = updatedAt;
        Value = value;
        Verified = verified;
        VerifiedAt = verifiedAt;
        Via = via;
    }

    /// <summary>
    /// When this entry was created
    /// </summary>
    /// <value>When this entry was created</value>
    /// <example>&quot;2014-01-01T23:28:56.782Z&quot;</example>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// The ID
    /// </summary>
    /// <value>The ID</value>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// VerifiableAddressStatus must not exceed 16 characters as that is the limitation in the SQL Schema
    /// </summary>
    /// <value>VerifiableAddressStatus must not exceed 16 characters as that is the limitation in the SQL Schema</value>
    [JsonPropertyName("status")]
    public string Status { get; set; }

    /// <summary>
    /// When this entry was last updated
    /// </summary>
    /// <value>When this entry was last updated</value>
    /// <example>&quot;2014-01-01T23:28:56.782Z&quot;</example>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// The address value  example foo@user.com
    /// </summary>
    /// <value>The address value  example foo@user.com</value>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// Indicates if the address has already been verified
    /// </summary>
    /// <value>Indicates if the address has already been verified</value>
    /// <example>true</example>
    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    /// <summary>
    /// Gets or Sets VerifiedAt
    /// </summary>
    [JsonPropertyName("verified_at")]
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// VerifiableAddressType must not exceed 16 characters as that is the limitation in the SQL Schema
    /// </summary>
    /// <value>VerifiableAddressType must not exceed 16 characters as that is the limitation in the SQL Schema</value>
    [JsonPropertyName("via")]
    public string Via { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class VerifiableIdentityAddress {\n");
        sb.Append("  CreatedAt: ").Append(CreatedAt).Append('\n');
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  Status: ").Append(Status).Append('\n');
        sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append('\n');
        sb.Append("  Value: ").Append(Value).Append('\n');
        sb.Append("  Verified: ").Append(Verified).Append('\n');
        sb.Append("  VerifiedAt: ").Append(VerifiedAt).Append('\n');
        sb.Append("  Via: ").Append(Via).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }
}

/// <summary>
/// RecoveryIdentityAddress
/// </summary>
public class RecoveryIdentityAddress
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RecoveryIdentityAddress" /> class.
    /// </summary>
    /// <param name="createdAt">CreatedAt is a helper struct field for gobuffalo.pop.</param>
    /// <param name="id">id</param>
    /// <param name="updatedAt">UpdatedAt is a helper struct field for gobuffalo.pop.</param>
    /// <param name="value">value</param>
    /// <param name="via">via</param>
    [JsonConstructor]
    public RecoveryIdentityAddress(DateTime createdAt, Guid id, DateTime updatedAt, string value, string via)
    {
        CreatedAt = createdAt;
        Id = id;
        UpdatedAt = updatedAt;
        Value = value;
        Via = via;
    }

    /// <summary>
    /// CreatedAt is a helper struct field for gobuffalo.pop.
    /// </summary>
    /// <value>CreatedAt is a helper struct field for gobuffalo.pop.</value>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// UpdatedAt is a helper struct field for gobuffalo.pop.
    /// </summary>
    /// <value>UpdatedAt is a helper struct field for gobuffalo.pop.</value>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or Sets Value
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; }

    /// <summary>
    /// Gets or Sets Via
    /// </summary>
    [JsonPropertyName("via")]
    public string Via { get; set; }

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("class RecoveryIdentityAddress {\n");
        sb.Append("  CreatedAt: ").Append(CreatedAt).Append('\n');
        sb.Append("  Id: ").Append(Id).Append('\n');
        sb.Append("  UpdatedAt: ").Append(UpdatedAt).Append('\n');
        sb.Append("  Value: ").Append(Value).Append('\n');
        sb.Append("  Via: ").Append(Via).Append('\n');
        sb.Append("}\n");
        return sb.ToString();
    }
}
