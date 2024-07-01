import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, ButtonToolbar,  Panel, Button, FlexboxGrid, Content } from 'rsuite';
import { useUser } from '../../Components/UserContext/UserContext';
import config                              from '..//../Utils/config';
//import tokenService from '..//../Services/tokenService';

interface LoginFormProps { }

interface LoginRequest{
    username: string;
    password: string;
}

const LoginPage: React.FC<LoginFormProps> = () => {
    const [username, setEmail      ] = useState('');
    const [password, setPassword] = useState('');
    const [credentials, setCredentials] = useState<LoginRequest>();
    const { login, setUser} = useUser();
    const navigate = useNavigate();

    const handleSubmit =  () => {
        try{
            const credentials: LoginRequest = { username, password };
            handleLogin(credentials);
        }
        catch (error) {
            console.error('Login failed:', error);
        }
    };

    const handleLogin = async (credentials: LoginRequest) => 
        {
            const response = await fetch(`${config.ApiBaseUrlDev}/account/Login`, {
                method: 'POST',
                body: JSON.stringify(credentials),
                headers: {
                  'Content-Type': 'application/json'
                }
              });
              if (response.ok) {
                  const data = await response.json();
                  if (data) {
                      //tokenService.saveUserResponse(data);
                      login(data);
                      setUser(data);
                      navigate('/');  
                  }
                  else {
                      alert('Login failed');
                    }
              }
        };

    
    return (
        <Content className='content'>
        <div className="login-page">
            <FlexboxGrid justify="center">
                <FlexboxGrid.Item colspan={6}>
                    <Panel header={<h3>Вход</h3>} bordered>
                        <Form fluid onSubmit={handleSubmit}>
                            <Form.Group controlId="email">
                                <Form.ControlLabel>Почта</Form.ControlLabel>
                                <Form.Control
                                    name="email"
                                    //type="email"
                                    value={username}
                                    onChange={value => setEmail(value)}
                                />
                                <Form.HelpText>Обязательно</Form.HelpText>
                                </Form.Group>
                            <Form.Group controlId="password">
                                <Form.ControlLabel>Пароль</Form.ControlLabel>
                                <Form.Control
                                    name="password"
                                    type="password"
                                    value={password}
                                    autoComplete="off"
                                    onChange={value => setPassword(value)}
                                />
                                <Form.HelpText>Обязательно</Form.HelpText>
                            </Form.Group>
                            <Form.Group>
                                <ButtonToolbar>
                                    <Button appearance="primary" onClick={handleSubmit}>Войти</Button>
                                    <Button appearance="link">Забыли пароль?</Button>
                                    <Button appearance="default">Отмена</Button>
                                </ButtonToolbar>
                            </Form.Group>
                        </Form>                        
                    </Panel>
                </FlexboxGrid.Item>
            </FlexboxGrid>
        </div>
        </Content>
    );
};

export default LoginPage;