package br.com.diego.agenda.bean;

import java.io.Serializable;

import javax.faces.bean.ManagedBean;
import javax.faces.bean.ViewScoped;

import org.omnifaces.util.Messages;

import br.com.diego.agenda.entity.Person;

@SuppressWarnings("serial")
@ManagedBean
@ViewScoped
public class PersonBean implements Serializable {
	
	private Person person;
	
	public Person getPerson() {
		return person;
	}
	
	public void setPerson(Person person) {
		this.person = person;
	}
	
	public void news() {
		person = new Person();
	}
	
	public void save(){
		Messages.addGlobalInfo("Programação Web com Java");
	}
}