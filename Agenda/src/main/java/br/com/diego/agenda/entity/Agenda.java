package br.com.diego.agenda.entity;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;

@SuppressWarnings("serial")
@Entity
public class Agenda extends GenericEntity {
	@Column(length = 100, nullable = false)
	private String annotation;

	@ManyToOne
	@JoinColumn(nullable = false)
	private Person person;

	public String getAnnotation() {
		return annotation;
	}

	public void setAnnotation(String annotation) {
		this.annotation = annotation;
	}

	public Person getPerson() {
		return person;
	}

	public void setPerson(Person person) {
		this.person = person;
	}
	
	

}
