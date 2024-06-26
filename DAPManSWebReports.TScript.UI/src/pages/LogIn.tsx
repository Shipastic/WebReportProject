import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, ButtonToolbar,  Panel, Button, FlexboxGrid } from 'rsuite';
import { useUser } from '../Components/UserContext/UserContext';
interface LoginFormProps { }

const Login: React.FC<LoginFormProps> = () => {
    const [email, setEmail      ] = useState('');
    const [password, setPassword] = useState('');

    const { setUser } = useUser();
    const navigate = useNavigate();

    const handleSubmit = () => {
        console.log('Email:', email);
        console.log('Password:', password);
        // Устанавливаем логин как текущего пользователя
        setUser(email);
        // Перенаправляем на главную страницу  
        navigate('/');  
    };

    return (
        <div className="login-page">
            <FlexboxGrid justify="center">
                <FlexboxGrid.Item colspan={6}>
                    <Panel header={<h3>Login</h3>} bordered>
                        <Form fluid onSubmit={handleSubmit}>
                            <Form.Group controlId="email">
                                <Form.ControlLabel>Email</Form.ControlLabel>
                                <Form.Control
                                    name="email"
                                    type="email"
                                    value={email}
                                    onChange={value => setEmail(value)}
                                />
                                <Form.HelpText>Required</Form.HelpText>
                                </Form.Group>
                            <Form.Group controlId="password">
                                <Form.ControlLabel>Password</Form.ControlLabel>
                                <Form.Control
                                    name="password"
                                    type="password"
                                    value={password}
                                    autoComplete="off"
                                    onChange={value => setPassword(value)}
                                />
                                <Form.HelpText>Required</Form.HelpText>
                            </Form.Group>
                            <Form.Group>
                                <ButtonToolbar>
                                    <Button appearance="primary" onClick={handleSubmit}>Sign in</Button>
                                    <Button appearance="link">Forgot password?</Button>
                                    <Button appearance="default">Cancel</Button>
                                </ButtonToolbar>
                            </Form.Group>
                        </Form>
                    </Panel>
                </FlexboxGrid.Item>
            </FlexboxGrid>
        </div>
    );
};

export default Login;