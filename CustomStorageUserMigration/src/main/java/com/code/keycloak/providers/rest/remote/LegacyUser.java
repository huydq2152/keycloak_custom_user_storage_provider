package com.code.keycloak.providers.rest.remote;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.annotation.JsonInclude;
import com.fasterxml.jackson.annotation.JsonProperty;

import java.util.List;
import java.util.Map;

/**
 * A user in the old authentication system
 */
@JsonIgnoreProperties(ignoreUnknown = true)
@JsonInclude(JsonInclude.Include.NON_NULL)
public record LegacyUser(
        String id,
        @JsonProperty("userName") String username,
        String email,
        String firstName,
        String lastName,
        @JsonProperty("isActive") boolean enabled,
        @JsonProperty("emailConfirmed") boolean emailVerified,

        Map<String, List<String>> attributes,

        List<String> roles,
        List<String> groups,
        List<String> requiredActions,
        List<LegacyTotp> totps
) {
}