import React, { useState } from 'react';
import { Container, Header, Content, Footer, Form, ButtonToolbar, FormGroup, FormControl, Navbar, Panel, Button, FlexboxGrid } from 'rsuite';

interface LoginFormProps { }

const Login: React.FC<LoginFormProps> = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = () => {
        console.log('Email:', email);
        console.log('Password:', password);
    };

    return (
        <div className="show-fake-browser login-page">
            <Container>               
                <Content>
                    <FlexboxGrid justify="center">
                        <FlexboxGrid.Item colspan={8}>
                            <Panel header={<h3>Login</h3>} bordered>
                                <Form fluid>
                                    <Form.Group>
                                        <Form.ControlLabel>Username or email address</Form.ControlLabel>
                                        <Form.Control name="name" />
                                    </Form.Group>
                                    <Form.Group>
                                        <Form.ControlLabel>Password</Form.ControlLabel>
                                        <Form.Control name="password" type="password" autoComplete="off" />
                                    </Form.Group>
                                    <Form.Group>
                                        <ButtonToolbar>
                                            <Button appearance="primary">Sign in</Button>
                                            <Button appearance="link">Forgot password?</Button>
                                        </ButtonToolbar>
                                    </Form.Group>
                                </Form>
                            </Panel>
                        </FlexboxGrid.Item>
                    </FlexboxGrid>
                </Content>
               
            </Container>
        </div>
    );
};

export default Login;