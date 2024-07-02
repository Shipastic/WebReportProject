import Keycloak from 'keycloak-js';

    const keycloakService = new Keycloak({
        url: 'https://your-keycloak-server/auth',
        realm: 'realm',
        clientId: 'client-id'
    });

    export default keycloakService;