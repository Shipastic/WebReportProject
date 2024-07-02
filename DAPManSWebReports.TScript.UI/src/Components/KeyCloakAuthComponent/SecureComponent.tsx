/*
import React, { useEffect } from 'react';
import { useKeycloak } from '@react-keycloak/web';

const SecureComponent = () => {
    const { keycloak, initialized } = useKeycloak();
    useEffect(() => {
        if (initialized && !keycloak.authenticated) {
            keycloak.login();
        }
    }, [keycloak, initialized]);
    return (
        <div>
            {initialized && keycloak.authenticated ?
                <p>User is authenticated</p> :
                <p>Loading...</p>}
        </div>
    );
};
export default SecureComponent;
*/