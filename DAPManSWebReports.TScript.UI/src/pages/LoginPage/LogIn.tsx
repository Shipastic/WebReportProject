import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Form, ButtonToolbar,  Panel, Button, FlexboxGrid, Content } from 'rsuite';
import { useUser } from '../../Components/UserContext/UserContext';
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
                                    type="email"
                                    value={email}
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

export default Login;