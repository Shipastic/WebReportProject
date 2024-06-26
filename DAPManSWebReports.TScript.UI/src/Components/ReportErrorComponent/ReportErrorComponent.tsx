import React, { useState } from 'react';
import { Button, Form, Message, Panel, Uploader, Input} from 'rsuite';
import config                              from '..//..//Utils/config';

const ReportErrorComponent: React.FC = () => {
  const [description, setDescription] = useState('');
  const [email, setEmail] = useState('');
  const [file, setFile] = useState(null);
  const [showSuccess, setShowSuccess] = useState(false);
  const [showError, setShowError] = useState(false);

  const handleSubmit = async () => {
    if (!description) {
      setShowError(true);
      return;
    }

    const formData = new FormData();
    formData.append('description', description);
    formData.append('email', email);
    if (file) {
      formData.append('file', file);
    }
    formData.append('url', window.location.href);

    try {
      const response = await fetch(`${config.ApiBaseUrlDev}/senderror`, {
        method: 'POST',
        body: formData,
      });
      
      if (response.ok) {
        setShowSuccess(true);
        setDescription('');
        setEmail('');
        setFile(null);
      } else {
        setShowError(true);
      }
    } catch (error) {
      setShowError(true);
    }
  };

  return (
    
    <Panel header={<h3 style={{textAlign:'center', color:'red'}}>Сообщить об ошибке</h3>} style={{backgroundColor:'rgba(31,97,141, 0.2)'}} bordered>
      <Form fluid>
        <Form.Group controlId="description">
          <Form.ControlLabel>Описание ошибки</Form.ControlLabel>
          <Input
            as="textarea"
            value={description}
            onChange={value => setDescription(value)}
            rows={5}
            placeholder="Пожалуйста, опишите проблему, с которой вы столкнулись"
            required
          />
        </Form.Group>
        <Form.Group controlId="email">
          <Form.ControlLabel>Email (необязательно)</Form.ControlLabel>
          <Input
            type="email"
            value={email}
            onChange={value => setEmail(value)}
          />
        </Form.Group>
        <Form.Group controlId="file">
          <Form.ControlLabel>Скриншот ошибки (необязательно)</Form.ControlLabel>
          <Uploader
            listType="picture-text"
            action={null}
            autoUpload={false}
            onChange={(fileList) => setFile(fileList[0]?.blobFile)}
          />
        </Form.Group>
        <Form.Group>
          <Button appearance="primary" onClick={handleSubmit}>Отправить</Button>
        </Form.Group>
      </Form>
      {showSuccess && <Message type="success" >Спасибо за обратную связь!</Message>}
      {showError && <Message type="error">Произошла ошибка при отправке.Пожалуйста, попробуйте еще раз.</Message>}
    </Panel>
  );
};

export default ReportErrorComponent;